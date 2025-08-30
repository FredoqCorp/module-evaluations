using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Result for a single criterion within a form run.
/// </summary>
public sealed record RunCriterionScore
{
    /// <summary>
    /// Criterion identifier.
    /// </summary>
    public required CriterionId CriterionId { get; init; }

    /// <summary>
    /// Whether the criterion is skipped (excluded from calculation).
    /// </summary>
    public bool Skipped { get; init; }

    /// <summary>
    /// Assessment for the criterion. If null and Skipped=false — not yet assessed.
    /// </summary>
    public CriterionAssessment? Assessment { get; init; }
}
