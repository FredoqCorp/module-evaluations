using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Behavioral contract for a groups collection that enforces weighted siblings.
/// </summary>
public interface IWeightedGroups : IGroups
{
    /// <summary>
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the groups expressed in basis points.</returns>
    IBasisPoints Weight();
}
