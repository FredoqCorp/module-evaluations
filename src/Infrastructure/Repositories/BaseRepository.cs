using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Provides a base implementation for repository classes using a database context factory.
/// </summary>
public class BaseRepository
{
    /// <summary>
    /// Gets or sets the factory for creating database context instances.
    /// </summary>
    protected IDbContextFactory<DatabaseContext> ContextFactory { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository"/> class with the specified database context factory.
    /// </summary>
    /// <param name="contextFactory">The factory for creating database context instances.</param>
    protected BaseRepository(IDbContextFactory<DatabaseContext> contextFactory)
    {
        ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
    }
}
