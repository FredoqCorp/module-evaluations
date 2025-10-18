using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;

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
