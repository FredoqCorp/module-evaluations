using System.Collections.Generic;
using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Immutable entity that encapsulates the criterion groups collection belonging to a form.
/// </summary>
public sealed class Groups : IGroups
{
    private readonly ImmutableList<IGroup> _items;

    /// <summary>
    /// Initializes the groups collection from the provided sequence.
    /// </summary>
    /// <param name="groups">Sequence of criterion groups.</param>
    public Groups(IEnumerable<IGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(groups);

        _items = [.. groups];
    }

    /// <summary>
    /// Calculates the total contribution produced by all groups.
    /// </summary>
    /// <returns>Total contribution from all groups.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution total = new RatingContribution(decimal.Zero, 0);

        foreach (var group in _items)
        {
            total = total.Join(group.Contribution());
        }

        return total;
    }

    /// <summary>
    /// Validates the internal consistency of the groups collection.
    /// </summary>
    public void Validate()
    {
        foreach (var group in _items)
        {
            group.Validate();
        }
    }
}
