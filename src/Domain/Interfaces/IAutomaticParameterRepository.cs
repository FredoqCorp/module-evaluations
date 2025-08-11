using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;

namespace CascVel.Module.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Repository interface for managing <see cref="AutomaticParameter"/> entities.
/// </summary>
public interface IAutomaticParameterRepository : IRepository<AutomaticParameter>
{
    /// <summary>
    /// Retrieves a list of automatic parameters asynchronously.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="AutomaticParameter"/>.</returns>
    Task<IEnumerable<AutomaticParameter>> GetListAsync(CancellationToken ct = default);
}
