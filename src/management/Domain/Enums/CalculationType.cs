namespace CascVel.Modules.Evaluations.Management.Domain.Enums;

/// <summary>
/// Represents the type of calculation rule applied at the form root level.
/// </summary>
public enum CalculationType
{
    /// <summary>
    /// Arithmetic average calculation without weights.
    /// </summary>
    Average = 0,

    /// <summary>
    /// Weighted average calculation where members have assigned weights.
    /// </summary>
    WeightedAverage = 1
}
