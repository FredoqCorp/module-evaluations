using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for a run-level group captured at launch time.
/// Holds metadata copied from the original form group and run-level children.
/// </summary>
public interface IRunFormGroup
{
    /// <summary>
    /// Returns the stable identifier of the group.
    /// </summary>
    IId Id();

    /// <summary>
    /// Returns the human-friendly title of the group.
    /// </summary>
    string Title();

    /// <summary>
    /// Returns the display order of the group in the form.
    /// </summary>
    IOrderIndex Order();


    /// <summary>
    /// Returns the run-level decorated criteria inside this group.
    /// </summary>
    IImmutableList<IRunFormCriterion> Criteria();

    /// <summary>
    /// Returns the run-level nested groups inside this group.
    /// </summary>
    IImmutableList<IRunFormGroup> Groups();
}
