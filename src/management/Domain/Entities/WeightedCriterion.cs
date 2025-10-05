using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Decorator that applies a weight to a criterion score.
/// </summary>
public sealed class WeightedCriterion : ICriterion
{
    private readonly ICriterion _criterion;
    private readonly IWeight _weight;

    /// <summary>
    /// Creates a weighted criterion decorator.
    /// </summary>
    /// <param name="criterion">The criterion to decorate.</param>
    /// <param name="weight">The weight to apply to the criterion score.</param>
    public WeightedCriterion(ICriterion criterion, IWeight weight)
    {
        _criterion = criterion;
        _weight = weight;
    }


    /// <summary>
    /// Calculates the weighted contribution based on the criterion's contribution and the weight.
    /// </summary>
    /// <returns>The weighted contribution that represents this criterion.</returns>
    public IRatingContribution Contribution()
    {
        return _weight.Weighted(_criterion.Contribution());
    }

}
