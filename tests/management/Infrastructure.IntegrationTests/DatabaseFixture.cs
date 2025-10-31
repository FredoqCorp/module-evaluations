using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Testcontainers.PostgreSql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests;

/// <summary>
/// Provides a PostgreSQL database container for integration tests with automatic migrations.
/// </summary>
public sealed class DatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _container;

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
    /// Stops and disposes the database container.
    /// </summary>
    public async Task DisposeAsync()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }
}
