using System.Collections.Immutable;
using System;
using System.Threading;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;

/// <summary>
/// Immutable collection of weighted groups that aggregates their contributions and sibling weight.
/// </summary>
public sealed class WeightedGroups : IWeightedGroups
{
    private IImmutableList<IWeightedGroup> _items;

    /// <summary>
    /// Initializes the weighted groups collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of weighted groups.</param>
    public WeightedGroups(IEnumerable<IWeightedGroup> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Adds a new weighted group to the form root level.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted group.</returns>
    public Task<IWeightedGroup> Add(GroupProfile profile, FormId formId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(weight);

        IWeightedCriteria criteria = new WeightedCriteria(Array.Empty<IWeightedCriterion>());
        IWeightedGroups groups = new WeightedGroups(Array.Empty<IWeightedGroup>());
        var group = new WeightedCriterionGroup(profile, criteria, groups, weight);
        _items = _items.Add(group);
        return Task.FromResult<IWeightedGroup>(group);
    }

    /// <summary>
    /// Adds a new weighted group to a parent group.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="parentId">Identifier of the parent group.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted group.</returns>
    public Task<IWeightedGroup> Add(GroupProfile profile, GroupId parentId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(weight);

        IWeightedCriteria criteria = new WeightedCriteria(Array.Empty<IWeightedCriterion>());
        IWeightedGroups groups = new WeightedGroups(Array.Empty<IWeightedGroup>());
        var group = new WeightedCriterionGroup(profile, criteria, groups, weight);
        _items = _items.Add(group);
        return Task.FromResult<IWeightedGroup>(group);
    }

    /// <summary>
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the groups expressed in basis points.</returns>
    public IBasisPoints Weight()
    {
        var points = 0;

        foreach (var item in _items)
        {
            var basis = item.Weight().Percent().Basis();
            points += decimal.ToInt32(basis.Apply(10000m));

            if (points > 10000)
            {
                throw new InvalidOperationException("Group sibling weights exceed one hundred percent");
            }
        }

        return new BasisPoints((ushort)points);
    }

    /// <summary>
    /// Validates the internal consistency of the weighted groups collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }
    }
}
