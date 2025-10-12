using System.Data;
using Npgsql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Database;

/// <summary>
/// Implements unit of work pattern using ADO.NET transactions.
/// </summary>
internal sealed class PostgresUnitOfWork : IUnitOfWork
{
    private readonly NpgsqlConnection _connection;

    private NpgsqlTransaction? _transaction;

    /// <summary>
    /// Initializes the unit of work with the provided connection string.
    /// </summary>
    /// <param name="connectionString">PostgreSQL connection string.</param>
    public PostgresUnitOfWork(string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        _connection = new NpgsqlConnection(connectionString);
    }

    /// <inheritdoc />
    public async Task<IDbConnection> ActiveConnection(CancellationToken ct = default)
    {
        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(ct);
        }

        return _connection;
    }

    /// <inheritdoc />
    public async Task BeginAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            throw new InvalidOperationException("Transaction already started");
        }

        if (_connection.State != ConnectionState.Open)
        {
            await _connection.OpenAsync(ct);
        }

        _transaction = await _connection.BeginTransactionAsync(ct);
    }

    /// <inheritdoc />
    public Task CommitAsync(CancellationToken ct = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Transaction not started");
        }

        return _transaction.CommitAsync(ct);
    }

    /// <inheritdoc />
    public Task RollbackAsync(CancellationToken ct = default)
    {
        if (_transaction is null)
        {
            throw new InvalidOperationException("Transaction not started");
        }

        return _transaction.RollbackAsync(ct);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}
