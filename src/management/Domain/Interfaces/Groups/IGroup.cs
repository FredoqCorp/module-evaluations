namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Behavioral contract for a single group.
/// </summary>
public interface IGroup
{
    /// <summary>
    /// Validates the internal consistency of the group.
    /// </summary>
    void Validate();
}
