using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

/// <summary>
/// Comparison rule for parameter against an evaluation
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConditionType
{
    /// <summary>Lower value is better</summary>
    LessIsBetter,
    /// <summary>Higher value is better</summary>
    MoreIsBetter,
}