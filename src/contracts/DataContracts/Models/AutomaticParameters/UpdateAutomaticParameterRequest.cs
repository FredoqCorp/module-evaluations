using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.AutomaticParameters;

/// <summary>
/// Request body to update an automatic parameter
/// </summary>
public sealed record UpdateAutomaticParameterRequest(
    [Required] string ServiceCode,
    [Required] string Caption,
    [EnumDataType(typeof(ConditionType))] ConditionType ConditionType
);


