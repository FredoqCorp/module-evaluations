using System;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Immutable unweighted group that aggregates unweighted criteria and subgroups.
/// </summary>
public sealed class AverageCriterionGroup : IAverageGroup
{
    private readonly GroupProfile _profile;
    private readonly IAverageCriteria _criteria;
    private readonly IAverageGroups _groups;

    /// <summary>
    /// Initializes the unweighted group with identity and members.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="criteria">Unweighted criteria belonging to the group.</param>
    /// <param name="groups">Unweighted sub-groups belonging to the group.</param>
    public AverageCriterionGroup(GroupProfile profile, IAverageCriteria criteria, IAverageGroups groups)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        _profile = profile;
        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Calculates the contribution produced by unweighted members of the group.
    /// </summary>
    /// <returns>Contribution represented by the group.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution total = new RatingContribution(decimal.Zero, 0);

        total = total.Join(_criteria.Contribution());
        total = total.Join(_groups.Contribution());

        return total;
    }

    /// <summary>
    /// Validates the internal consistency of the unweighted group.
    /// </summary>
    public void Validate()
    {
        _criteria.Validate();
        _groups.Validate();
    }
}
