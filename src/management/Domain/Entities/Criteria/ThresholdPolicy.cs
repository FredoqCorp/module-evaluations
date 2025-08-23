namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Policy describing how thresholds on options should be interpreted.
/// </summary>
public sealed class ThresholdPolicy
{
    /// <summary>
    /// Optimization goal: minimize or maximize KPI.
    /// </summary>
    public required OptimizationGoal Goal { get; init; }
}
