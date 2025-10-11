using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Database;

/// <summary>
/// Integration tests for unit of work pattern implementation.
/// </summary>
public sealed class UnitOfWorkTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly DbConnectionFactory _factory;

    public UnitOfWorkTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _factory = new DbConnectionFactory(_fixture.ConnectionString);
    }

    [Fact]
    public async Task UnitOfWork_begins_transaction_successfully()
    {
        await using var uow = new UnitOfWork(_factory);

        await uow.BeginAsync();

        // If no exception thrown, transaction started successfully
        Assert.True(true);
    }

    [Fact]
    public async Task UnitOfWork_commits_transaction_successfully()
    {
        await using var uow = new UnitOfWork(_factory);

        await uow.BeginAsync();
        await uow.CommitAsync();

        // If no exception thrown, transaction committed successfully
        Assert.True(true);
    }

    [Fact]
    public async Task UnitOfWork_rolls_back_transaction_successfully()
    {
        await using var uow = new UnitOfWork(_factory);

        await uow.BeginAsync();
        await uow.RollbackAsync();

        // If no exception thrown, transaction rolled back successfully
        Assert.True(true);
    }

    [Fact]
    public async Task UnitOfWork_throws_when_committing_without_transaction()
    {
        await using var uow = new UnitOfWork(_factory);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await uow.CommitAsync());

        Assert.Contains("Transaction not started", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UnitOfWork_throws_when_rolling_back_without_transaction()
    {
        await using var uow = new UnitOfWork(_factory);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await uow.RollbackAsync());

        Assert.Contains("Transaction not started", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task UnitOfWork_throws_when_beginning_transaction_twice()
    {
        await using var uow = new UnitOfWork(_factory);

        await uow.BeginAsync();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await uow.BeginAsync());

        Assert.Contains("Transaction already started", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void UnitOfWork_throws_when_factory_is_null()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new UnitOfWork(null!));

        Assert.Equal("factory", exception.ParamName);
    }
}
