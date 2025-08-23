using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

/// <summary>
/// Rule to calculate a final form score
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CalculationRule
{
    /// <summary>Average of all scores</summary>
    Average,
    /// <summary>Weighted average of scores</summary>
    WeightedAverage,
}