using System.Data;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Database;

/// <summary>
/// Implements unit of work pattern using ADO.NET transactions.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory _factory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(IDbConnectionFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <inheritdoc />
    public async Task BeginAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("Transaction already started");
        }

        _connection = await _factory.CreateAsync(cancellationToken);
        _transaction = _connection.BeginTransaction();
    }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Transaction not started");
        }

        _transaction.Commit();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Transaction not started");
        }

        _transaction.Rollback();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            _transaction.Dispose();
            _transaction = null;
        }

        if (_connection is not null)
        {
            _connection.Dispose();
            _connection = null;
        }

        return ValueTask.CompletedTask;
    }
}
