namespace CascVel.Module.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Criterion assessment: selected score, comment, and optional automation evidence.
/// Absence of an instance for a criterion means the criterion is skipped.
/// </summary>
public sealed class CriterionAssessment
{
    /// <summary>
    /// Selected score (Option.Score).
    /// </summary>
    public required ushort SelectedScore { get; init; }

    /// <summary>
    /// Comment for the criterion.
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// Evidence for automatic criterion, if applicable.
    /// </summary>
    public RunAutoEvidence? Auto { get; init; }
}
