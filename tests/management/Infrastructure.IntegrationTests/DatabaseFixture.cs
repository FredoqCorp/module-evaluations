using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Npgsql;
using Testcontainers.PostgreSql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests;

/// <summary>
/// Provides a PostgreSQL database container for integration tests with automatic migrations.
/// </summary>
public sealed class DatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _container;
    private NpgsqlDataSource? _source;

    /// <summary>
    /// Gets the PostgreSQL connection string.
    /// </summary>
    public string ConnectionString { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes the database container and runs migrations.
    /// </summary>
    public async Task InitializeAsync()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:18-alpine")
            .WithDatabase("evaluations_test")
            .WithUsername("test_user")
            .WithPassword("test_password")
            .Build();

        await _container.StartAsync();

        ConnectionString = _container.GetConnectionString();
        _source = NpgsqlDataSource.Create(ConnectionString);

        // Run DbUp migrations
        var result = DatabaseMigrator.MigrateDatabase(ConnectionString);

        if (!result.Successful)
        {
            throw new InvalidOperationException(
                $"Database migration failed: {result.Error}",
                result.Error);
        }
    }

    /// <summary>
    /// Resets all mutable tables to provide clean state between tests.
    /// </summary>
    /// <returns>Asynchronous operation.</returns>
    public async Task Reset()
    {
        if (_source is null)
        {
            throw new InvalidOperationException("Database fixture is not initialized");
        }

        await using var connection = await _source.OpenConnectionAsync();
        await Truncate(connection);
    }

    /// <summary>
    /// Stops and disposes the database container.
    /// </summary>
    public async Task DisposeAsync()
    {
        if (_source is not null)
        {
            await _source.DisposeAsync();
        }

        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    /// <summary>
    /// Truncates all public tables while preserving schema history.
    /// </summary>
    /// <param name="connection">Active PostgreSQL connection.</param>
    /// <returns>Asynchronous operation.</returns>
    private static async Task Truncate(NpgsqlConnection connection)
    {
        ArgumentNullException.ThrowIfNull(connection);

        const string sql = """
            DO $$
            DECLARE
                stmt text;
            BEGIN
                SELECT 'TRUNCATE TABLE ' || string_agg(format('%I.%I', table_schema, table_name), ', ') || ' RESTART IDENTITY CASCADE;'
                INTO stmt
                FROM information_schema.tables
                WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
                  AND table_type = 'BASE TABLE'
                  AND table_name <> 'schemaversions';
                IF stmt IS NOT NULL THEN
                    EXECUTE stmt;
                END IF;
            END $$;
            """;

        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
