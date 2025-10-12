using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;

/// <summary>
/// Response object containing a collection of form summaries.
/// </summary>
public sealed record ListFormsResponse
{
    /// <summary>
    /// Initializes the response with the provided collection of form summaries.
    /// </summary>
    /// <param name="forms">Collection of form summaries.</param>
    public ListFormsResponse(IImmutableList<IFormSummary> forms)
    {
        ArgumentNullException.ThrowIfNull(forms);

        Forms = forms;
    }

    /// <summary>
    /// Collection of form summaries retrieved from the repository.
    /// </summary>
    public IImmutableList<IFormSummary> Forms { get; init; }
}
