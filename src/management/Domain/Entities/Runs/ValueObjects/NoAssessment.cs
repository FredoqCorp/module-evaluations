using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Null Object implementation of ICriterionAssessment representing absence of a selected score.
/// </summary>
public sealed record NoAssessment : ICriterionAssessment
{
    /// <summary>
    /// Returns false indicating the assessment does not contain a selected score.
    /// </summary>
    public bool Present() => false;

    /// <summary>
    /// Throws because there is no selected score when assessment is absent.
    /// </summary>
    public ushort SelectedScore()
    {
        throw new InvalidOperationException("Assessment is absent");
    }

    /// <summary>
    /// Returns an empty comment string for the absent assessment.
    /// </summary>
    public string Comment() => string.Empty;
}

