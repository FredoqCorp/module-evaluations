using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.AutomaticParameters;

/// <summary>
/// Response model representing an automatic parameter
/// </summary>
public sealed record AutomaticParameterResponse(
    Guid Id,
    string ServiceCode,
    string Caption,
    ConditionType ConditionType
);