using System.Collections.Immutable;
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
    /// <returns>List of form summaries.</returns>
    Task<IImmutableList<IFormSummary>> List(CancellationToken ct = default);
}
