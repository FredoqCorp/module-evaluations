using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

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
    IId FormId();

    /// <summary>
    /// Returns human-facing metadata captured at launch time.
    /// </summary>
    IFormMeta Meta();

    /// <summary>
    /// Returns the immutable form code captured at launch time.
    /// </summary>
    string Code();

    /// <summary>
    /// Returns the calculation rule kind captured at launch time.
    /// </summary>
    FormCalculationKind Rule();

    /// <summary>
    /// Returns the ordered groups of criteria captured at launch time.
    /// </summary>
    IImmutableList<IRunFormGroup> Groups();

    /// <summary>
    /// Returns the root-level criteria captured at launch time.
    /// </summary>
    IImmutableList<IRunFormCriterion> Criteria();

}
