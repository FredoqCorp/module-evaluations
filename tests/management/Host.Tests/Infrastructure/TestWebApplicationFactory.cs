using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    public string ConnectionString => _postgres.GetConnectionString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Set connection string before host is built
        builder.UseSetting("ConnectionStrings:DefaultConnection", ConnectionString);

        builder.ConfigureTestServices(services =>
        {
            // Additional test service configuration if needed
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
    }

    public new async Task DisposeAsync()
    {
        await _postgres.DisposeAsync();
        await base.DisposeAsync();
    }
}
