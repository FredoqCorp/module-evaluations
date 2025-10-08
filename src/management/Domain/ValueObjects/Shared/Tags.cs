using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

/// <summary>
/// Immutable collection of tags that preserves order and prevents case-insensitive duplicates.
/// </summary>
public sealed record Tags : ITags
{
    private readonly ImmutableList<Tag> _tags;

    /// <summary>
    /// Initializes the collection from the provided sequence while enforcing uniqueness invariant.
    /// </summary>
    /// <param name="tags">Sequence of tags that forms the initial collection.</param>
    public Tags(IEnumerable<Tag> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);
        _tags = [.. tags];
    }

    /// <summary>
    /// Returns a new collection that includes the provided tag after verifying uniqueness.
    /// </summary>
    /// <param name="tag">Tag candidate to include.</param>
    /// <returns>New collection with the candidate appended.</returns>
    public ITags With(Tag tag)
    {
        if (_tags.Contains(tag))
        {
            return this;
        }

        return new Tags(_tags.Add(tag));   
    }
}
