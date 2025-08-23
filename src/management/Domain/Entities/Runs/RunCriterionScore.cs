namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Result for a single criterion within a form run.
/// </summary>
public sealed class RunCriterionScore
{
    /// <summary>
    /// Criterion identifier.
    /// </summary>
    public required long CriterionId { get; init; }

    /// <summary>
    /// Whether the criterion is skipped (excluded from calculation).
    /// </summary>
    public bool Skipped { get; init; }

    /// <summary>
    /// Assessment for the criterion. If null and Skipped=false — not yet assessed.
    /// </summary>
    public CriterionAssessment? Assessment { get; init; }
}
