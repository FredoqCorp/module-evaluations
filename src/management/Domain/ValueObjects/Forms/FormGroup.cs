using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// A group of criteria within a form with title, order and ordered references.
/// </summary>
public sealed record FormGroup
{
    /// <summary>
    /// Creates a form group with title, order, weight, criteria and nested groups.
    /// </summary>
    public FormGroup(FormGroupId id, string title, OrderIndex order, FormCriteriaList criteria, FormGroupList groups)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        Id = id;
        Title = title;
        Order = order;
        Criteria = criteria;
        Groups = groups;
    }

    /// <summary>
    /// Returns the stable identifier of the group.
    /// </summary>
    public FormGroupId Id { get; }

    /// <summary>
    /// Returns the human-friendly title of the group.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Returns the display order of the group in the form.
    /// </summary>
    public OrderIndex Order { get; }

    /// <summary>
    /// Returns the ordered criteria inside the group.
    /// </summary>
    public FormCriteriaList Criteria { get; }

    /// <summary>
    /// Returns the nested groups inside this group.
    /// </summary>
    public FormGroupList Groups { get; }
}
