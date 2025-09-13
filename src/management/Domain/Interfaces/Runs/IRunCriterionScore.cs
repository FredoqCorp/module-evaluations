using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
/// <summary>
/// Contract for a per-criterion result in a form run.
/// </summary>
public interface IRunCriterionScore
{
    /// <summary>
    /// Returns the run-level decorated criterion captured at launch.
    /// </summary>
    FormCriterionId CriterionId();

    /// <summary>
    /// Returns whether the criterion is skipped.
    /// </summary>
    bool Skipped();

    /// <summary>
    /// Returns the criterion assessment.
    /// </summary>
    ICriterionAssessment Assessment();
}
