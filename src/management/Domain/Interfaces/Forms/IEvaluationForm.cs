using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for the evaluation form aggregate root.
/// </summary>
public interface IEvaluationForm
{
    /// <summary>
    /// Returns the lifecycle value object.
    /// </summary>
    FormLifecycle Lifecycle();

    /// <summary>
    /// Returns the ordered groups of criteria.
    /// </summary>
    IImmutableList<IFormGroup> Groups();

    /// <summary>
    /// Returns the criteria outside any group.
    /// </summary>
    IImmutableList<IFormCriterion> Criteria();

    /// <summary>
    /// Returns a run form snapshot for this evaluation form using the stored calculation policy definition.
    /// </summary>
    IRunFormSnapshot Snapshot();
}
