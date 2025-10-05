using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Fixtures;

/// <summary>
/// xUnit fixture that starts a PostgreSQL 16-alpine container, applies EF Core migrations, and exposes services and reset capability.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
public sealed class PostgresFixture : IAsyncLifetime
#pragma warning restore CA1515 // Consider making public types internal
{
    private readonly PostgreSqlContainer _container;
    private string _connectionString = string.Empty;
    private Respawner? _respawner;

    /// <summary>
    /// Service provider configured with the module DbContext factory
    /// </summary>
    public IServiceProvider Services { get; private set; } = default!;

    /// <summary>
    /// Connection string to the running PostgreSQL container
    /// </summary>
    public string ConnectionString => _connectionString;

    /// <summary>
    /// Initializes a new PostgreSQL container definition
    /// </summary>
    public PostgresFixture()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("module_evaluations_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }

    /// <summary>
    /// Starts the container, applies EF Core migrations, and prepares Respawn
    /// </summary>
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _connectionString = _container.GetConnectionString();

        var sc = new ServiceCollection();
        sc.AddLogging(b => b.SetMinimumLevel(LogLevel.None));
        Services = sc.BuildServiceProvider();

        await using var scope = Services.CreateAsyncScope();

    }

    /// <summary>
    /// Resets database state using Respawn while keeping the schema
    /// </summary>
    public async Task ResetAsync()
    {
        if (_respawner == null)
        {
            return;
        }
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
    }

#pragma warning disable S1144 // Unused private types or members should be removed
    private async Task PrepareRespawnerAsync(DbConnection connection)
#pragma warning restore S1144 // Unused private types or members should be removed
    {
        await connection.OpenAsync();
        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            SchemasToInclude = ["evaluations"],
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = [new Table("evaluations", "__EFMigrationsHistory")],
            CheckTemporalTables = false,
        });
        await connection.CloseAsync();
    }

    /// <summary>
    /// Disposes the PostgreSQL container
    /// </summary>
    public async Task DisposeAsync() => await _container.DisposeAsync();
}
