using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

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
}
