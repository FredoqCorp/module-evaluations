namespace CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;

/// <summary>
/// Use case for retrieving a list of form summaries.
/// </summary>
public interface IListForms
{
    /// <summary>
    /// Executes the use case to retrieve all form summaries.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Response containing the collection of form summaries.</returns>
    Task<ListFormsResponse> Execute(CancellationToken ct = default);
}
