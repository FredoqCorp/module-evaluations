namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Contract for a group of criteria within a form.
/// </summary>
public interface IFormGroup
{
    /// <summary>
    /// Returns the title string.
    /// </summary>
    string Title();

    /// <summary>
    /// Returns the display order index value object.
    /// </summary>
    IOrderIndex Order();

    /// <summary>
    /// Returns the group weight object.
    /// </summary>
    IWeight Weight();

    /// <summary>
    /// Returns the ordered criteria inside this group.
    /// </summary>
    IImmutableList<IFormCriterion> Criteria();

    /// <summary>
    /// Returns the nested groups inside this group.
    /// </summary>
    IImmutableList<IFormGroup> Groups();
}
