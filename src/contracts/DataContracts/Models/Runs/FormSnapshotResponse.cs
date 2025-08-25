using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs;

/// <summary>
/// Represents a snapshot of the form structure at run time including groups and criteria
/// </summary>
/// <param name="Id">Unique numeric form identifier</param>
/// <param name="Data">Form data containing all fields and values</param>
/// <param name="FormCriteria">Form criteria</param>
/// <param name="FormGroups">Form groups</param>
public sealed record FormSnapshotResponse(
    long Id,
    FormData Data,
    IReadOnlyList<FormCriterionViewResponse> FormCriteria,
    IReadOnlyList<FormGroupResponse> FormGroups
);
