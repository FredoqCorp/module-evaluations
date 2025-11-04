namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Represents a readable title for a group definition.
/// </summary>
public interface IGroupTitle
{
    /// <summary>
    /// Reads the group title string.
    /// </summary>
    /// <returns>Group title string.</returns>
    string Text();
}
