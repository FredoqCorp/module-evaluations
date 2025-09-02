using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run-level group with copied metadata and run-level children for the snapshot structure.
/// </summary>
public sealed record RunFormGroup : IRunFormGroup
{
    private readonly Guid _key;
    private readonly string _title;
    private readonly IOrderIndex _order;
    private readonly IWeight _weight;
    private readonly IImmutableList<IRunFormCriterion> _criteria;
    private readonly IImmutableList<IRunFormGroup> _groups;

    /// <summary>
    /// Creates a run-level group with key, metadata and run-level children.
    /// </summary>
    public RunFormGroup(Guid key, string title, IOrderIndex order, IWeight weight, IImmutableList<IRunFormCriterion> criteria, IImmutableList<IRunFormGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(weight);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);
        _key = key;
        _title = title;
        _order = order;
        _weight = weight;
        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Returns the run-local unique key of this group within the snapshot.
    /// </summary>
    public Guid Key() => _key;

    /// <summary>
    /// Returns the human-friendly title of the group.
    /// </summary>
    public string Title() => _title;

    /// <summary>
    /// Returns the display order of the group in the form.
    /// </summary>
    public IOrderIndex Order() => _order;

    /// <summary>
    /// Returns the weight of the group.
    /// </summary>
    public IWeight Weight() => _weight;

    /// <summary>
    /// Returns the run-level criteria inside the group.
    /// </summary>
    public IImmutableList<IRunFormCriterion> Criteria() => _criteria;

    /// <summary>
    /// Returns the run-level nested groups inside this group.
    /// </summary>
    public IImmutableList<IRunFormGroup> Groups() => _groups;
}
