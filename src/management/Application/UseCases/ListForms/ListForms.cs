using System.Diagnostics.CodeAnalysis;
using CascVel.Modules.Evaluations.Management.Application.Ports;

namespace CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;

/// <summary>
/// Use case for retrieving a list of form summaries.
/// </summary>
[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI container")]
internal sealed class ListForms : IListForms
{
    private readonly IForms _forms;

    /// <summary>
    /// Initializes the use case with the forms repository port.
    /// </summary>
    /// <param name="forms">Repository port for accessing form data.</param>
    public ListForms(IForms forms)
    {
        ArgumentNullException.ThrowIfNull(forms);

        _forms = forms;
    }

    /// <summary>
    /// Executes the use case to retrieve all form summaries.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Response containing the collection of form summaries.</returns>
    public async Task<ListFormsResponse> Execute(CancellationToken ct = default)
    {
        var summaries = await _forms.List(ct);

        return new ListFormsResponse(summaries);
    }
}
