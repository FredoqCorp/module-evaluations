using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Represents a group of criteria used for evaluations.
/// </summary>
public sealed class CriterionGroup : ICriterion
{
    private readonly GroupId _id;
    private readonly GroupTitle _title;
    private readonly GroupDescription _description;
    private readonly IImmutableList<ICriterion> _criteria;

    /// <summary>
    /// Creates a new criterion group with the specified properties.
    /// </summary>
    /// <param name="id">Unique identifier for the group.</param>
    /// <param name="title">Title of the group.</param>
    /// <param name="description">Description of the group.</param>
    /// <param name="criteria">Immutable list of criteria in the group.</param>
    public CriterionGroup(
        GroupId id,
        GroupTitle title,
        GroupDescription description,
        IImmutableList<ICriterion> criteria)
    {
        _id = id;
        _title = title;
        _description = description;
        _criteria = criteria;
    }

    /// <summary>
    /// Calculates the total contribution produced by all criteria in the group.
    /// </summary>
    /// <returns>The contribution that represents the group.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution totalContribution = new RatingContribution(decimal.Zero, 0);

        foreach (var criterion in _criteria)
        {
            totalContribution = totalContribution.Join(criterion.Contribution());
        }

        return totalContribution;
    }

}
