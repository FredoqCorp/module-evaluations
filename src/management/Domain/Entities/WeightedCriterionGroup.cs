using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Decorator that applies a weight to a criterion group score.
/// </summary>
public sealed class WeightedCriterionGroup : ICriterion
{
    private readonly ICriterion _group;
    private readonly IWeight _weight;

    /// <summary>
    /// Creates a weighted criterion group decorator.
    /// </summary>
    /// <param name="group">The criterion group to decorate.</param>
    /// <param name="weight">The weight to apply to the group score.</param>
    public WeightedCriterionGroup(ICriterion group, IWeight weight)
    {
        _group = group;
        _weight = weight;
    }

    /// <summary>
    /// Calculates the weighted contribution based on the group's contribution and the weight.
    /// </summary>
    /// <returns>The weighted contribution that represents this group.</returns>
    public IRatingContribution Contribution()
    {
        return _weight.Weighted(_group.Contribution());
    }

}
