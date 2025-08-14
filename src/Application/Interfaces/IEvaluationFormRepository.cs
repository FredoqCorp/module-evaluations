using CascVel.Module.Evaluations.Management.Domain.Entities.Form;
using CascVel.Module.Evaluations.Management.Domain.Enums.Forms;

namespace CascVel.Module.Evaluations.Management.Application.Interfaces;

/// <summary>
/// Repository interface for managing <see cref="EvaluationForm"/> entities.
/// </summary>
public interface IEvaluationFormRepository : IRepository<EvaluationForm>
{
    /// <summary>
    /// Retrieves an <see cref="EvaluationForm"/> by its unique code.
    /// </summary>
    /// <param name="code">The unique code of the evaluation form.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>The <see cref="EvaluationForm"/> if found; otherwise, null.</returns>
    Task<EvaluationForm?> GetByCode(string code, CancellationToken ct = default);

    /// <summary>
    /// Retrieves a list of all <see cref="EvaluationForm"/> entities.
    /// </summary>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>An enumerable collection of <see cref="EvaluationForm"/> entities.</returns>
    Task<IEnumerable<EvaluationForm>> GetListAsync(CancellationToken ct = default);

    /// <summary>
    /// Sets the status of an <see cref="EvaluationForm"/> by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the evaluation form.</param>
    /// <param name="status">The new status to set for the evaluation form.</param>
    /// <param name="user">The user performing the status update.</param>
    /// <param name="ct">A cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetStatusAsync(long id, EvaluationFormStatus status, string user, CancellationToken ct = default);

    /// <summary>
    /// Asynchronously updates an existing automatic parameter in the repository.
    /// </summary>
    /// <param name="entity">The evaluation form to update.</param>
    /// <param name="ct">A cancellation token.</param>
    Task UpdateAsync(EvaluationForm entity, CancellationToken ct = default);


}
