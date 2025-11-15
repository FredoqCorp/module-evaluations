using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Port for managing evaluation forms.
/// </summary>
public interface IForms
{
    /// <summary>
    /// Retrieves summary information for all evaluation forms.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Printable collection of form summaries.</returns>
    Task<IFormSummaries> List(CancellationToken ct = default);

    /// <summary>
    /// Persists a fully described form aggregate.
    /// </summary>
    /// <param name="form">Printable form aggregate.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The same form aggregate provided to the method.</returns>
    Task<IForm> Add(IForm form, CancellationToken ct = default);

    /// <summary>
    /// Loads a fully described form aggregate by its identifier.
    /// </summary>
    /// <param name="id">Identifier describing the target form.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Printable form aggregate retrieved from persistence.</returns>
    Task<IForm> Form(FormId id, CancellationToken ct = default);
}
