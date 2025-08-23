using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a group of form criteria and subgroups in a form to create.
/// </summary>
/// <param name="Title">The title of the group.</param>
/// <param name="Order">The display order of the group.</param>
/// <param name="Weight">The optional weight of the group.</param>
/// <param name="FormCriteria">The list of criteria in the group.</param>
/// <param name="SubGroups">The list of subgroups within this group.</param>
public sealed record CreateFormGroupRequest(
    [Required] string Title,
    [Required] ushort Order,
    [Required] decimal? Weight,
    [Required] IReadOnlyList<CreateFormCriterionViewRequest> FormCriteria,
    [Required] IReadOnlyList<CreateFormGroupRequest> SubGroups
);
