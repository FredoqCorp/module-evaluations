namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Contract for a per-criterion result in a form run.
/// </summary>
public interface IRunCriterionScore
{
    /// <summary>
    /// Returns the criterion identifier.
    /// </summary>
    Uuid CriterionId();

    /// <summary>
    /// Returns whether the criterion is skipped.
    /// </summary>
    bool Skipped();

    /// <summary>
    /// Returns the criterion assessment when present.
    /// </summary>
    ICriterionAssessment? Assessment();
}

