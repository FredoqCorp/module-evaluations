using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;

/// <summary>
/// Immutable collection of weighted groups that aggregates their contributions and sibling weight.
/// </summary>
public sealed class WeightedGroups : IWeightedGroups
{
    private readonly IImmutableList<IWeightedGroup> _items;

    /// <summary>
    /// Initializes the weighted groups collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of weighted groups.</param>
    public WeightedGroups(IEnumerable<IWeightedGroup> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the groups expressed in basis points.</returns>
    public IBasisPoints Weight()
    {
        var points = 0;

        foreach (var item in _items)
        {
            var basis = item.Weight().Percent().Basis();
            points += decimal.ToInt32(basis.Apply(10000m));

            if (points > 10000)
            {
                throw new InvalidOperationException("Group sibling weights exceed one hundred percent");
            }
        }

        return new BasisPoints((ushort)points);
    }

    /// <summary>
    /// Validates the internal consistency of the weighted groups collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }
    }
}
