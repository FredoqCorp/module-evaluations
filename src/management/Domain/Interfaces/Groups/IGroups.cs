using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Interface for Groups
/// </summary>
public interface IGroups
{
    /// <summary>
    /// Validates the internal consistency of the groups aggregate.
    /// </summary>
    void Validate();
}
