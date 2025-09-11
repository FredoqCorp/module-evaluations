using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// A group of criteria within a form with title, order and ordered references.
/// </summary>
public sealed record FormGroup
{
    private readonly FormGroupId _id;
    private readonly string _title;
    private readonly OrderIndex _order;
    private readonly IImmutableList<FormCriterion> _criteria;
    private readonly IImmutableList<FormGroup> _groups;

    /// <summary>
    /// Creates a form group with title, order, weight, criteria and nested groups.
    /// </summary>
    public FormGroup(FormGroupId id, string title, OrderIndex order, IImmutableList<FormCriterion> criteria, IImmutableList<FormGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(title);
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
    public FormGroupId Id()
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
    public OrderIndex Order()
    {
        return _order;
    }

    

    /// <summary>
    /// Returns the ordered criteria inside the group.
    /// </summary>
    public IImmutableList<FormCriterion> Criteria()
    {
        return _criteria;
    }

    /// <summary>
    /// Returns the nested groups inside this group.
    /// </summary>
    public IImmutableList<FormGroup> Groups()
    {
        return _groups;
    }
}
