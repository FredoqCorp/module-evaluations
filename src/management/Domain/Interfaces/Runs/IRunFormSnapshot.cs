using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for an immutable snapshot of a form captured at run launch time.
/// Used for stable addressing of criteria and reproducible recalculation.
/// </summary>
public interface IRunFormSnapshot
{
    /// <summary>
    /// Returns the unique identifier of the form.
    /// </summary>
    EvaluationFormId FormId();

    /// <summary>
    /// Returns human-facing metadata captured at launch time.
    /// </summary>
    FormMeta Meta();

    /// <summary>
    /// Returns the bound runtime calculation policy captured at launch time.
    /// </summary>
    ICalculationPolicy Policy();

    /// <summary>
    /// Returns the ordered groups of criteria captured at launch time.
    /// </summary>
    IImmutableList<FormGroup> Groups();

    /// <summary>
    /// Returns the root-level criteria captured at launch time.
    /// </summary>
    IImmutableList<FormCriterion> Criteria();

}
