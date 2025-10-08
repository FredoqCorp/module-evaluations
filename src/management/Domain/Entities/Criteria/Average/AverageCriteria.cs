using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;

/// <summary>
/// Immutable collection of unweighted criteria that participate in average scoring.
/// </summary>
public sealed class AverageCriteria : IAverageCriteria
{
    private readonly IImmutableList<IAverageCriterion> _items;

    /// <summary>
    /// Initializes the unweighted criteria collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of unweighted criteria.</param>
    public AverageCriteria(IEnumerable<IAverageCriterion> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Calculates the total contribution produced by all unweighted criteria.
    /// </summary>
    /// <returns>Total contribution from all unweighted criteria.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution total = new RatingContribution(decimal.Zero, 0);

        foreach (var item in _items)
        {
            total = total.Join(item.Contribution());
        }

        return total;
    }

    /// <summary>
    /// Validates the internal consistency of the unweighted criteria collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }
    }
}
