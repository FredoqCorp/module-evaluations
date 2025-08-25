namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Aggregated result data: current total score and per-criterion scores.
/// </summary>
public sealed class RunResult
{
    /// <summary>
    /// Current calculated total score of the form (captured on each save).
    /// </summary>
    public decimal? CurrentTotal { get; init; }

    /// <summary>
    /// Criterion scores for the run.
    /// </summary>
    public required IReadOnlyList<RunCriterionScore> Criteria { get; init; }
}
