using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a response containing form data, status, and audit information
/// </summary>
/// <param name="Id">Unique numeric form identifier</param>
/// <param name="Data">Form data containing all fields and values</param>
/// <param name="Status">Current form status</param>
/// <param name="CreatedBy">Login of the user who created the form</param>
/// <param name="CreatedAt">Creation date</param>
/// <param name="ModifiedBy">Login of the user who modified the form</param>
/// <param name="ModifiedAt">Modification date</param>
/// <param name="StatusChangedBy">Login of the user who changed the form status</param>
/// <param name="StatusChangedAt">Date time of the last status change</param>
/// <param name="FormCriteria">Form criteria</param>
/// <param name="FormGroups">Form groups</param>
public sealed record FormResponse(
    long Id,
    FormData Data,
    FormStatus Status,
    string CreatedBy,
    DateTime CreatedAt,
    string? ModifiedBy,
    DateTime? ModifiedAt,
    string? StatusChangedBy,
    DateTime? StatusChangedAt,
    IReadOnlyList<FormCriterionViewResponse> FormCriteria,
    IReadOnlyList<FormGroupResponse> FormGroups
);