namespace CascVel.Module.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Defines generic repository operations for entities of type <typeparamref name="T"/>.
/// </summary>
public interface IRepository<T>
{
    /// <summary>
    /// Asynchronously creates a new entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The created entity.</returns>
    Task<T> CreateAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously retrieves an entity by its identifier.
    /// </summary>
    /// <param name="entityId">The identifier of the entity to retrieve.</param>
    /// <param name="isFullInclude">Whether to include related entities in the result.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The entity with the specified identifier.</returns>
    Task<T> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The updated entity.</returns>
    Task<T> UpdateAsync(T entity, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously deletes an entity by its identifier.
    /// </summary>
    /// <param name="entityId">The identifier of the entity to delete.</param>
    /// <param name="ct">A cancellation token.</param>
    Task DeleteAsync(long entityId, CancellationToken ct = default);
}
