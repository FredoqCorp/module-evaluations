using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Immutable entity that encapsulates the criteria collection belonging to a form.
/// </summary>
public sealed class Criteria : ICriteria
{
    private readonly IImmutableList<IRatingContributionSource> _items;

    /// <summary>
    /// Initializes the criteria collection from the provided sequence.
    /// </summary>
    /// <param name="criteria">Sequence of criteria.</param>
    public Criteria(IEnumerable<IRatingContributionSource> criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        _items = [.. criteria];
    }

    /// <summary>
    /// Calculates the total contribution produced by all criteria.
    /// </summary>
    /// <returns>Total contribution from all criteria.</returns>
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
    /// Validates the internal consistency of the criteria collection.
    /// </summary>
    public void Validate()
    {
    }
}
