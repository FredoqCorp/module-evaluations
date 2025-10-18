using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;

/// <summary>
/// Immutable criterion that stores a weight together with the decorated criterion.
/// </summary>
public sealed class WeightedCriterion : IWeightedCriterion
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
        ArgumentNullException.ThrowIfNull(criterion);
        ArgumentNullException.ThrowIfNull(weight);

        _criterion = criterion;
        _weight = weight;
    }

    /// <summary>
    /// Returns the weight associated with the criterion.
    /// </summary>
    /// <returns>Weight assigned to the criterion.</returns>
    public IWeight Weight()
    {
        return _weight;
    }

    /// <summary>
    /// Validates the internal consistency of the criterion.
    /// </summary>
    public void Validate()
    {
        _criterion.Validate();
    }
}
