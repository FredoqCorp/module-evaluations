namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using System.Collections.Immutable;

/// <summary>
/// A group of criteria within a form with title, order, optional weight and ordered references.
/// </summary>
public sealed record FormGroup : IFormGroup
{
    private readonly string _title;
    private readonly IOrderIndex _order;
    private readonly IWeight _weight;
    private readonly IImmutableList<IFormCriterion> _criteria;
    private readonly IImmutableList<IFormGroup> _groups;

    /// <summary>
    /// Creates a form group with title, order, weight, criteria and nested groups.
    /// </summary>
    public FormGroup(string title, IOrderIndex order, IWeight weight, IImmutableList<IFormCriterion> criteria, IImmutableList<IFormGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(weight);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        _title = title;
        _order = order;
        _weight = weight;

        _criteria = criteria;
        _groups = groups;
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
    /// Returns the weight of the group.
    /// </summary>
    public IWeight Weight()
    {
        return _weight;
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
