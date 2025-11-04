namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

/// <summary>
/// Provides access to a single tag text representation.
/// </summary>
public interface ITag
{
    /// <summary>
    /// Reads the tag text.
    /// </summary>
    /// <returns>Tag text.</returns>
    string Text();
}
