namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;
using System.Collections.Immutable;

/// <summary>
/// A group of criteria within a form with title, order, optional weight and ordered references.
/// </summary>
public sealed record FormGroup : IFormGroup
{
    private readonly IId _id;
    private readonly string _title;
    private readonly IOrderIndex _order;
    private readonly IImmutableList<IFormCriterion> _criteria;
    private readonly IImmutableList<IFormGroup> _groups;

    /// <summary>
    /// Creates a form group with title, order, weight, criteria and nested groups.
    /// </summary>
    public FormGroup(IId id, string title, IOrderIndex order, IImmutableList<IFormCriterion> criteria, IImmutableList<IFormGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        _id = id;
        _title = title;
        _order = order;

        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Returns the stable identifier of the group.
    /// </summary>
    public IId Id()
    {
        return _id;
    }

    /// <summary>
    /// Returns the human-friendly title of the group.
    /// </summary>
    public string Title()
    {
        return _title;
    }

    /// <summary>
    /// Returns the display order of the group in the form.
    /// </summary>
    public IOrderIndex Order()
    {
        return _order;
    }

    

    /// <summary>
    /// Returns the ordered criteria inside the group.
    /// </summary>
    public IImmutableList<IFormCriterion> Criteria()
    {
        return _criteria;
    }

    /// <summary>
    /// Returns the nested groups inside this group.
    /// </summary>
    public IImmutableList<IFormGroup> Groups()
    {
        return _groups;
    }
}
