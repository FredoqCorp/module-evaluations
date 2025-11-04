using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Database;

/// <summary>
/// Integration tests for unit of work pattern implementation.
/// </summary>
public sealed class UnitOfWorkTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public UnitOfWorkTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UnitOfWork_begins_transaction_successfully()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        await uow.BeginAsync();

        // If no exception thrown, transaction started successfully
        Assert.True(true);
    }

    [Fact]
    public async Task UnitOfWork_commits_transaction_successfully()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        await uow.BeginAsync();
        await uow.CommitAsync();

        // If no exception thrown, transaction committed successfully
        Assert.True(true);
    }

    [Fact]
    public async Task UnitOfWork_rolls_back_transaction_successfully()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        await uow.BeginAsync();
        await uow.RollbackAsync();

        // If no exception thrown, transaction rolled back successfully
        Assert.True(true);
    }

    [Fact]
    public async Task UnitOfWork_throws_when_committing_without_transaction()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await uow.CommitAsync());

        Assert.Contains("Transaction not started", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UnitOfWork_throws_when_rolling_back_without_transaction()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await uow.RollbackAsync());

        Assert.Contains("Transaction not started", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UnitOfWork_throws_when_beginning_transaction_twice()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        await uow.BeginAsync();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await uow.BeginAsync());

        Assert.Contains("Transaction already started", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void UnitOfWork_throws_when_connection_string_is_null()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new PostgresUnitOfWork(null!));

        Assert.Equal("connectionString", exception.ParamName);
    }

    [Fact]
    public void UnitOfWork_throws_when_connection_string_is_empty()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new PostgresUnitOfWork(string.Empty));

        Assert.Equal("connectionString", exception.ParamName);
    }

    [Fact]
    public async Task ActiveConnection_returns_open_connection()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        var connection = await uow.ActiveConnection();

        Assert.NotNull(connection);
        Assert.Equal(System.Data.ConnectionState.Open, connection.State);
    }

    [Fact]
    public async Task ActiveConnection_returns_same_connection_on_multiple_calls()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);

        var connection1 = await uow.ActiveConnection();
        var connection2 = await uow.ActiveConnection();

        Assert.Same(connection1, connection2);
    }

    [Fact]
    public async Task Transaction_commits_changes_to_database()
    {
        await _fixture.Reset();
        var testId = Guid.NewGuid();

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        await uow.BeginAsync();

        var connection = await uow.ActiveConnection();
        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = testId,
                Name = "Test Form",
                Code = "TEST",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await uow.CommitAsync();

        // Verify the form was committed
        await using var verifyUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var verifyConnection = await verifyUow.ActiveConnection();
        var count = await verifyConnection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM forms WHERE id = @Id",
            new { Id = testId });

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task Transaction_rollback_discards_changes()
    {
        await _fixture.Reset();
        var testId = Guid.NewGuid();

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        await uow.BeginAsync();

        var connection = await uow.ActiveConnection();
        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = testId,
                Name = "Test Form",
                Code = "TEST",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await uow.RollbackAsync();

        // Verify the form was NOT committed
        await using var verifyUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var verifyConnection = await verifyUow.ActiveConnection();
        var count = await verifyConnection.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM forms WHERE id = @Id",
            new { Id = testId });

        Assert.Equal(0, count);
    }
}
