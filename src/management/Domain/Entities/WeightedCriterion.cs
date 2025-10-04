using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

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
    /// Calculates the weighted score based on the criterion's score and the weight.
    /// </summary>
    /// <returns>The weighted criterion score if the criterion has a score; otherwise, None.</returns>
    public Option<CriterionScore> Score() => _criterion
            .Score()
            .Map(_weight.Weighted);
}
