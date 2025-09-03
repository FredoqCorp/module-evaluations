namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

using System.Collections.Immutable;

/// <summary>
/// Contract for aggregated result data of a form run.
/// </summary>
public interface IRunResult
{
    /// <summary>
    /// Returns the current calculated total score.
    /// </summary>
    decimal CurrentTotal();

    /// <summary>
    /// Returns the per-criterion scores.
    /// </summary>
    IImmutableList<IRunCriterionScore> Criteria();
}
