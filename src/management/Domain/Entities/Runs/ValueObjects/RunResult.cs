using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Aggregated result data: current total score and per-criterion scores as an immutable value object.
/// </summary>
public sealed record RunResult : IRunResult
{
    private readonly decimal? _currentTotal;
    private readonly IImmutableList<RunCriterionScore> _criteria;

    /// <summary>
    /// Creates a run result with optional total and per-criterion scores.
    /// </summary>
    public RunResult(decimal? currentTotal, IImmutableList<RunCriterionScore> criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        _currentTotal = currentTotal;
        _criteria = criteria;
    }

    /// <summary>
    /// Returns the current calculated total score when present.
    /// </summary>
    public decimal? CurrentTotal() => _currentTotal;

    /// <summary>
    /// Returns the per-criterion scores.
    /// </summary>
    public IImmutableList<RunCriterionScore> Criteria() => _criteria;
}
