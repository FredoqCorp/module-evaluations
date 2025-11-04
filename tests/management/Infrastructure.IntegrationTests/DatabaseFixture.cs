using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests;

/// <summary>
/// Provides a PostgreSQL database container for integration tests with automatic migrations.
/// </summary>
public sealed class DatabaseFixture : IAsyncLifetime
{
    private static readonly string[] Schemas = ["public"];
    private static readonly Table[] IgnoredTables = [new("schemaversions")];

    private PostgreSqlContainer? _container;
    private NpgsqlDataSource? _source;
    private Respawner? _respawner;

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

        await PrepareRespawner();
    }

    /// <summary>
    /// Resets all mutable tables to provide clean state between tests.
    /// </summary>
    /// <returns>Asynchronous operation.</returns>
    public async Task Reset()
    {
        if (_source is null || _respawner is null)
        {
            throw new InvalidOperationException("Database fixture is not initialized");
        }

        await using var connection = await _source.OpenConnectionAsync();
        await _respawner.ResetAsync(connection);
    }

    /// <summary>
    /// Configures Respawn to truncate tables while preserving schema history.
    /// </summary>
    /// <returns>Asynchronous operation.</returns>
    private async Task PrepareRespawner()
    {
        if (_source is null)
        {
            throw new InvalidOperationException("Database source is not initialized");
        }

        await using var connection = await _source.OpenConnectionAsync();
        _respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = Schemas,
                WithReseed = true,
                TablesToIgnore = IgnoredTables
            });
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
}
