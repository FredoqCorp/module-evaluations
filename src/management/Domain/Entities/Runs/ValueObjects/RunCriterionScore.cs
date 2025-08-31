using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Result for a single criterion within a form run as an immutable value object.
/// </summary>
public sealed record RunCriterionScore : IRunCriterionScore
{
    private readonly Uuid _criterionId;
    private readonly bool _skipped;
    private readonly ICriterionAssessment? _assessment;

    /// <summary>
    /// Creates a per-criterion score with identifier, skipped flag and optional assessment.
    /// </summary>
    public RunCriterionScore(Uuid criterionId, bool skipped, ICriterionAssessment? assessment)
    {
        _criterionId = criterionId;
        _skipped = skipped;
        _assessment = assessment;
    }

    /// <summary>
    /// Returns the criterion identifier.
    /// </summary>
    public Uuid CriterionId() => _criterionId;

    /// <summary>
    /// Returns whether the criterion is skipped.
    /// </summary>
    public bool Skipped() => _skipped;

    /// <summary>
    /// Returns the criterion assessment when present.
    /// </summary>
    public ICriterionAssessment? Assessment() => _assessment;
}
