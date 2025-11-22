using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Npgsql;
using Testcontainers.PostgreSql;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Host.Tests.Infrastructure;

/// <summary>
/// Test web application factory for E2E tests with PostgreSQL container.
/// </summary>
public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:18-alpine")
        .WithDatabase("evaluations_test")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private NpgsqlDataSource? _source;

    /// <summary>
    /// Gets the PostgreSQL connection string for the test host.
    /// </summary>
    public string ConnectionString => _postgres.GetConnectionString();

    /// <summary>
    /// Configures the web host for integration testing.
    /// </summary>
    /// <param name="builder">Host builder instance.</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Set connection string before host is built
        builder.UseSetting("ConnectionStrings:DefaultConnection", ConnectionString);

        builder.ConfigureTestServices(services =>
        {
            // Disable authorization for tests - allow all requests through
            services.AddSingleton<IPolicyEvaluator, AllowAnonymousPolicyEvaluator>();
        });
    }

    /// <summary>
    /// Starts the PostgreSQL container and applies migrations.
    /// </summary>
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        // Run migrations
        var result = DatabaseMigrator.MigrateDatabase(ConnectionString);

        if (!result.Successful)
        {
            throw new InvalidOperationException(
                $"Database migration failed: {result.Error}");
        }

        _source = NpgsqlDataSource.Create(ConnectionString);
    }

    /// <summary>
    /// Resets the database state to guarantee test isolation.
    /// </summary>
    /// <returns>Asynchronous operation.</returns>
    public async Task Reset()
    {
        if (_source is null)
        {
            throw new InvalidOperationException("Web application factory is not initialized");
        }

        await using var connection = await _source.OpenConnectionAsync();
        await Truncate(connection);
    }

    /// <summary>
    /// Disposes managed resources used by the test host.
    /// </summary>
    public new async Task DisposeAsync()
    {
        if (_source is not null)
        {
            await _source.DisposeAsync();
        }

        await _postgres.DisposeAsync();
        await base.DisposeAsync();
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
