using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a request to create a new form with associated criteria
/// </summary>
/// <param name="Data">Data for the form to be created</param>
/// <param name="FormCriteria">Criteria to be created with the form</param>
/// <param name="FormGroups">Groups to be created with the form</param>
public sealed record CreateFormRequest(
    [Required] FormData Data,
    [Required] IReadOnlyList<CreateFormCriterionViewRequest> FormCriteria,
    [Required] IReadOnlyList<CreateFormGroupRequest> FormGroups
);
