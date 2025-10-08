using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Immutable collection of unweighted groups that participate in average scoring.
/// </summary>
public sealed class AverageGroups : IAverageGroups
{
    private readonly IImmutableList<IAverageGroup> _items;

    /// <summary>
    /// Initializes the unweighted groups collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of unweighted groups.</param>
    public AverageGroups(IEnumerable<IAverageGroup> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Calculates the total contribution produced by all unweighted groups.
    /// </summary>
    /// <returns>Total contribution from all unweighted groups.</returns>
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
    /// Validates the internal consistency of the unweighted groups collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }
    }
}
