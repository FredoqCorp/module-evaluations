namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a group of form criteria and subgroups in a form response.
/// </summary>
/// <param name="Id">The unique identifier of the group.</param>
/// <param name="Title">The title of the group.</param>
/// <param name="Order">The display order of the group.</param>
/// <param name="Weight">The optional weight of the group.</param>
/// <param name="FormCriteria">The list of criteria in the group.</param>
/// <param name="SubGroups">The list of subgroups within this group.</param>
public sealed record FormGroupResponse(
    long Id,
    string Title,
    ushort Order,
    decimal? Weight,
    IReadOnlyList<FormCriterionViewResponse> FormCriteria,
    IReadOnlyList<FormGroupResponse> SubGroups
);
