using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

/// <summary>
/// Behavioral contract for an immutable tags collection that prevents duplicates and returns new instances on change requests.
/// </summary>
public interface ITags
{
    /// <summary>
    /// Returns a new collection that includes the provided tag while enforcing uniqueness invariant.
    /// </summary>
    /// <param name="tag">Tag candidate to include.</param>
    /// <returns>New tags collection containing the candidate.</returns>
    ITags With(Tag tag);

    /// <summary>
    /// Prints the tags collection as a string array into the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <param name="key">Property name or key for the array.</param>
    void Print(IMedia media, string key);
}
