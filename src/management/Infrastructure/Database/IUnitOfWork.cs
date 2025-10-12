using System.Data;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Database;

/// <summary>
/// Represents a unit of work pattern for managing database transactions.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Returns the active database connection.
    /// </summary>
    /// <param name="ct"> Cancellation token.</param>
    Task<IDbConnection> ActiveConnection(CancellationToken ct = default);

    /// <summary>
    /// Begins a new transaction.
    /// </summary>
    /// <param name="ct"> Cancellation token.</param>
    Task BeginAsync(CancellationToken ct = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="ct"> Cancellation token.</param>
    Task CommitAsync(CancellationToken ct = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="ct"> Cancellation token.</param>
    Task RollbackAsync(CancellationToken ct = default);

}
