using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Represents a group of criteria used for evaluations.
/// </summary>
public sealed class CriterionGroup : IGroup
{
    private readonly GroupId _id;
    private readonly GroupTitle _title;
    private readonly GroupDescription _description;
    private readonly ICriteria _criteria;
    private readonly IGroups _groups;

    /// <summary>
    /// Creates a new criterion group with the specified properties.
    /// </summary>
    /// <param name="id">Unique identifier for the group.</param>
    /// <param name="title">Title of the group.</param>
    /// <param name="description">Description of the group.</param>
    /// <param name="criteria">Immutable list of criteria in the group.</param>
    /// <param name="groups">Immutable list of sub-groups in the group.</param>
    public CriterionGroup(
        GroupId id,
        GroupTitle title,
        GroupDescription description,
        ICriteria criteria,
        IGroups groups)
    {
        _id = id;
        _title = title;
        _description = description;
        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Calculates the total contribution produced by all criteria in the group.
    /// </summary>
    /// <returns>The contribution that represents the group.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution totalContribution = new RatingContribution(decimal.Zero, 0);

        totalContribution = totalContribution.Join(_criteria.Contribution());
        totalContribution = totalContribution.Join(_groups.Contribution());

        return totalContribution;
    }

    /// <summary>
    /// Validates the internal consistency of the single group.
    /// </summary>
    public void Validate()
    {
    }
}
