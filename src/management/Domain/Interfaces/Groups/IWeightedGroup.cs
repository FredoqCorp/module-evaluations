using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Behavioral contract for a group that exposes its weight towards parent calculations.
/// </summary>
public interface IWeightedGroup : IGroup
{
    /// <summary>
    /// Returns the weight associated with the group.
    /// </summary>
    /// <returns>Weight assigned to the group.</returns>
    IWeight Weight();
}
