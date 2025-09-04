namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

using System.Collections.Immutable;

/// <summary>
/// Contract for a group of criteria within a form.
/// </summary>
public interface IFormGroup
{
    /// <summary>
    /// Returns the stable identifier of the group.
    /// </summary>
    IId Id();
    /// <summary>
    /// Returns the title string.
    /// </summary>
    string Title();

    /// <summary>
    /// Returns the display order index value object.
    /// </summary>
    IOrderIndex Order();


    /// <summary>
    /// Returns the ordered criteria inside this group.
    /// </summary>
    IImmutableList<IFormCriterion> Criteria();

    /// <summary>
    /// Returns the nested groups inside this group.
    /// </summary>
    IImmutableList<IFormGroup> Groups();
}
