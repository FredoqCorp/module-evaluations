using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Application.Ports;

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
}
