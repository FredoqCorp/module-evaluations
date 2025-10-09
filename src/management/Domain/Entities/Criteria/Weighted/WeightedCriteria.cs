using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;

/// <summary>
/// Immutable collection of weighted criteria that aggregates their contributions and sibling weight.
/// </summary>
public sealed class WeightedCriteria : IWeightedCriteria
{
    private readonly IImmutableList<IWeightedCriterion> _items;

    /// <summary>
    /// Initializes the weighted criteria collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of weighted criteria.</param>
    public WeightedCriteria(IEnumerable<IWeightedCriterion> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Calculates the total contribution produced by all weighted criteria.
    /// </summary>
    /// <returns>Total contribution from all weighted criteria.</returns>
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
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the criteria expressed in basis points.</returns>
    public IBasisPoints Weight()
    {
        var points = 0;

        foreach (var item in _items)
        {
            var basis = item.Weight().Percent().Basis();
            points += decimal.ToInt32(basis.Apply(10000m));

            if (points > 10000)
            {
                throw new InvalidOperationException("Criteria sibling weights exceed one hundred percent");
            }
        }

        return new BasisPoints((ushort)points);
    }

    /// <summary>
    /// Validates the internal consistency of the weighted criteria collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }

        if (_items.Count == 0)
        {
            return;
        }

        var total = Weight().Apply(10000m);

        if (total != 10000m)
        {
            throw new InvalidOperationException("Criteria sibling weights must equal one hundred percent");
        }
    }
}
