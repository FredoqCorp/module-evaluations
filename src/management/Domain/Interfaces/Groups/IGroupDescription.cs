namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Represents a readable description for a group definition.
/// </summary>
public interface IGroupDescription
{
    /// <summary>
    /// Reads the group description string.
    /// </summary>
    /// <returns>Group description string.</returns>
    string Text();
}
