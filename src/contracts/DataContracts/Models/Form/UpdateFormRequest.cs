using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a request to update a form with its criteria.
/// </summary>
/// <param name="Id">The unique identifier of the form to update.</param>
/// <param name="Data">The new data for the form.</param>
/// <param name="FormCriteria">The list of criteria to update for the form.</param>
/// <param name="FormGroups">The list of groups to update for the form.</param>
public sealed record UpdateFormRequest([Required] long Id, [Required] FormData Data, [Required] IReadOnlyList<UpdateFormCriterionViewRequest> FormCriteria, [Required] IReadOnlyList<FormGroup> FormGroups);
