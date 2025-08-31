namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Contract for aggregated result data of a form run.
/// </summary>
public interface IRunResult
{
    /// <summary>
    /// Returns the current calculated total score when present.
    /// </summary>
    decimal? CurrentTotal();

    /// <summary>
    /// Returns the per-criterion scores.
    /// </summary>
    IImmutableList<RunCriterionScore> Criteria();
}

