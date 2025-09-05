using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Result for a single criterion within a form run as an immutable value object.
/// </summary>
public sealed record RunCriterionScore : IRunCriterionScore
{
    private readonly IFormCriterion _criterion;
    private readonly bool _skipped;
    private readonly ICriterionAssessment _assessment;

    /// <summary>
    /// Creates a per-criterion score with a snapshot reference, skipped flag and optional assessment.
    /// </summary>
    public RunCriterionScore(IFormCriterion criterion, bool skipped, ICriterionAssessment assessment)
    {
        ArgumentNullException.ThrowIfNull(criterion);
        ArgumentNullException.ThrowIfNull(assessment);
        _criterion = criterion;
        _skipped = skipped;
        _assessment = assessment;
    }

    /// <summary>
    /// Returns the criterion reference inside the launched form snapshot.
    /// </summary>
    public IFormCriterion Criterion() => _criterion;

    /// <summary>
    /// Returns whether the criterion is skipped.
    /// </summary>
    public bool Skipped() => _skipped;

    /// <summary>
    /// Returns the criterion assessment.
    /// </summary>
    public ICriterionAssessment Assessment() => _assessment;
}
