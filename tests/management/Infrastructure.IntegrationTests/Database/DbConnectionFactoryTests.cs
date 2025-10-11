using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using System.Data;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Database;

/// <summary>
/// Integration tests for database connection factory.
/// </summary>
public sealed class DbConnectionFactoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public DbConnectionFactoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Factory_creates_open_connection()
    {
        var factory = new DbConnectionFactory(_fixture.ConnectionString);

        using var connection = await factory.CreateAsync();

        Assert.Equal(ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task Factory_creates_multiple_independent_connections()
    {
        var factory = new DbConnectionFactory(_fixture.ConnectionString);

        using var connection1 = await factory.CreateAsync();
        using var connection2 = await factory.CreateAsync();

        Assert.NotSame(connection1, connection2);
        Assert.Equal(ConnectionState.Open, connection1.State);
        Assert.Equal(ConnectionState.Open, connection2.State);
    }

    [Fact]
    public void Factory_throws_when_connection_string_is_empty()
    {
        var exception = Assert.Throws<ArgumentException>(() => new DbConnectionFactory(string.Empty));

        Assert.Contains("Connection string cannot be empty", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Factory_throws_when_connection_string_is_whitespace()
    {
        var exception = Assert.Throws<ArgumentException>(() => new DbConnectionFactory("   "));

        Assert.Contains("Connection string cannot be empty", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Factory_allows_concurrent_connection_creation()
    {
        var factory = new DbConnectionFactory(_fixture.ConnectionString);

        var tasks = Enumerable.Range(0, 10)
            .Select(async _ =>
            {
                using var connection = await factory.CreateAsync();
                return connection.State;
            });

        var states = await Task.WhenAll(tasks);

        Assert.All(states, state => Assert.Equal(ConnectionState.Open, state));
    }
}
