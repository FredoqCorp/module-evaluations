using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Host.Tests.Infrastructure;

/// <summary>
/// Test web application factory for E2E tests with PostgreSQL container.
/// </summary>
public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly string[] Schemas = ["public"];
    private static readonly Table[] IgnoredTables = [new("schemaversions")];

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:18-alpine")
        .WithDatabase("evaluations_test")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private NpgsqlDataSource? _source;
    private Respawner? _respawner;

    public string ConnectionString => _postgres.GetConnectionString();

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
        await Prepare();
    }

    /// <summary>
    /// Resets the database state using Respawn to guarantee test isolation.
    /// </summary>
    /// <returns>Asynchronous operation.</returns>
    public async Task Reset()
    {
        if (_source is null || _respawner is null)
        {
            throw new InvalidOperationException("Web application factory is not initialized");
        }

        await using var connection = await _source.OpenConnectionAsync();
        await _respawner.ResetAsync(connection);
    }

    private async Task Prepare()
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

    public new async Task DisposeAsync()
    {
        if (_source is not null)
        {
            await _source.DisposeAsync();
        }

        await _postgres.DisposeAsync();
        await base.DisposeAsync();
    }
}
