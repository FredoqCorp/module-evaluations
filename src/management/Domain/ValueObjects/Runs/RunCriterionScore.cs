using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;

/// <summary>
/// Result for a single criterion within a form run as an immutable value object.
/// </summary>
public sealed record RunCriterionScore : IRunCriterionScore
{
    private readonly FormCriterion _criterion;
    private readonly bool _skipped;
    private readonly ICriterionAssessment _assessment;

    /// <summary>
    /// Creates a per-criterion score with a snapshot reference, skipped flag and optional assessment.
    /// </summary>
    public RunCriterionScore(FormCriterion criterion, bool skipped, ICriterionAssessment assessment)
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
    public FormCriterion Criterion() => _criterion;

    /// <summary>
    /// Returns whether the criterion is skipped.
    /// </summary>
    public bool Skipped() => _skipped;

    /// <summary>
    /// Returns the criterion assessment.
    /// </summary>
    public ICriterionAssessment Assessment() => _assessment;
}
