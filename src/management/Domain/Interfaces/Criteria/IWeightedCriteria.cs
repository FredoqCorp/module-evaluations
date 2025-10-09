using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Behavioral contract for a criteria collection that operates with weights.
/// </summary>
public interface IWeightedCriteria : ICriteria
{
    /// <summary>
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the criteria expressed in basis points.</returns>
    IBasisPoints Weight();
}
