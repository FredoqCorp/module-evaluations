using System.Data;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Database;

/// <summary>
/// Represents a factory for creating database connections.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates and opens a new database connection.
    /// </summary>
    Task<IDbConnection> CreateAsync(CancellationToken cancellationToken = default);
}
