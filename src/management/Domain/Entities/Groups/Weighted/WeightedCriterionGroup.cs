using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;

/// <summary>
/// Immutable criterion group that aggregates weighted members and exposes its own weight.
/// </summary>
public sealed class WeightedCriterionGroup : IWeightedGroup
{
    private readonly GroupProfile _profile;
    private readonly IWeightedCriteria _criteria;
    private readonly IWeightedGroups _groups;
    private readonly IWeight _weight;

    /// <summary>
    /// Initializes the weighted criterion group with identity, members, and weight.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="criteria">Weighted criteria belonging to the group.</param>
    /// <param name="groups">Weighted sub-groups belonging to the group.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    public WeightedCriterionGroup(
        GroupProfile profile,
        IWeightedCriteria criteria,
        IWeightedGroups groups,
        IWeight weight)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(weight);

        _profile = profile;
        _criteria = criteria;
        _groups = groups;
        _weight = weight;
    }

    /// <summary>
    /// Calculates the weighted contribution based on the members' contributions and the group weight.
    /// </summary>
    /// <returns>The weighted contribution that represents this group.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution total = new RatingContribution(decimal.Zero, 0);

        total = total.Join(_criteria.Contribution());
        total = total.Join(_groups.Contribution());

        return _weight.Weighted(total);
    }

    /// <summary>
    /// Returns the weight associated with the group.
    /// </summary>
    /// <returns>Weight assigned to the group.</returns>
    public IWeight Weight()
    {
        return _weight;
    }

    /// <summary>
    /// Validates the internal consistency of the weighted criterion group.
    /// </summary>
    public void Validate()
    {
        _criteria.Validate();
        _groups.Validate();

        var criteria = decimal.ToInt32(_criteria.Weight().Apply(10000m));
        var groups = decimal.ToInt32(_groups.Weight().Apply(10000m));

        if (criteria == 0 && groups == 0)
        {
            throw new InvalidOperationException("Weighted group must contain weighted members");
        }

        if (criteria > 0 && groups == 0 && criteria != 10000)
        {
            throw new InvalidOperationException("Criteria sibling weights must equal one hundred percent");
        }

        if (groups > 0 && criteria == 0 && groups != 10000)
        {
            throw new InvalidOperationException("Group sibling weights must equal one hundred percent");
        }

        if (criteria > 0 && groups > 0 && criteria + groups != 10000)
        {
            throw new InvalidOperationException("Criteria and groups combined weights must equal one hundred percent");
        }
    }
}
