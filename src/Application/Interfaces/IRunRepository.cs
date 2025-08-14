using CascVel.Module.Evaluations.Management.Domain.Entities.Filters;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Module.Evaluations.Management.Domain.Enums.Runs;

namespace CascVel.Module.Evaluations.Management.Application.Interfaces;

/// <summary>
/// Repository interface for managing Run entities.
/// </summary>
public interface IRunRepository : IRepository<Run>
{
    /// <summary>
    /// Retrieves a list of Run entities based on the specified filter.
    /// </summary>
    /// <param name="filter">The filter criteria for retrieving runs.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of Run entities.</returns>
    Task<IEnumerable<Run>> GetListAsync(RunFilter filter, CancellationToken ct = default);

    /// <summary>
    /// Publishes the specified run.
    /// </summary>
    /// <param name="id">The identifier of the run to publish.</param>
    /// <param name="userLogin">The login of the user performing the publish action.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    Task Publish(long id, string userLogin, CancellationToken ct = default);

    /// <summary>
    /// Sets the viewed state for the specified run.
    /// </summary>
    /// <param name="id">The identifier of the run to update.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetRunViewedState(long id, CancellationToken ct = default);

    /// <summary>
    /// Sets the approval state for the specified run.
    /// </summary>
    /// <param name="id">The identifier of the run to update.</param>
    /// <param name="status">The approval status to set for the run.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SetApproveState(long id, RunAgreementStatus status, CancellationToken ct = default);
}
