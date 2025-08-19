namespace CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Optimization goal for numeric scale.
/// </summary>
public enum OptimizationGoal
{
    /// <summary>
    /// Minimize the KPI (lower is better).
    /// </summary>
    Minimize = 0,

    /// <summary>
    /// Maximize the KPI (higher is better).
    /// </summary>
    Maximize = 1,
}
