using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;

/// <summary>
/// Immutable collection of unweighted groups that participate in average scoring.
/// </summary>
public sealed class AverageGroups : IAverageGroups
{
    private IImmutableList<IAverageGroup> _items;

    /// <summary>
    /// Initializes the unweighted groups collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of unweighted groups.</param>
    public AverageGroups(IEnumerable<IAverageGroup> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Adds a new average group to the form root level.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added average group.</returns>
    public Task<IAverageGroup> Add(GroupProfile profile, FormId formId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        IAverageCriteria criteria = new AverageCriteria(Array.Empty<IAverageCriterion>());
        IAverageGroups groups = new AverageGroups(Array.Empty<IAverageGroup>());
        var group = new AverageCriterionGroup(profile, criteria, groups);
        _items = _items.Add(group);
        return Task.FromResult<IAverageGroup>(group);
    }

    /// <summary>
    /// Adds a new average group to a parent group.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="parentId">Identifier of the parent group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added average group.</returns>
    public Task<IAverageGroup> Add(GroupProfile profile, GroupId parentId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        IAverageCriteria criteria = new AverageCriteria(Array.Empty<IAverageCriterion>());
        IAverageGroups groups = new AverageGroups(Array.Empty<IAverageGroup>());
        var group = new AverageCriterionGroup(profile, criteria, groups);
        _items = _items.Add(group);
        return Task.FromResult<IAverageGroup>(group);
    }

    /// <summary>
    /// Validates the internal consistency of the unweighted groups collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }
    }
}
