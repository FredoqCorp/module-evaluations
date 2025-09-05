using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
/// <summary>
/// Contract for a per-criterion result in a form run.
/// </summary>
public interface IRunCriterionScore
{
    /// <summary>
    /// Returns the run-level decorated criterion captured at launch.
    /// </summary>
    IFormCriterion Criterion();

    /// <summary>
    /// Returns whether the criterion is skipped.
    /// </summary>
    bool Skipped();

    /// <summary>
    /// Returns the criterion assessment.
    /// </summary>
    ICriterionAssessment Assessment();
}
