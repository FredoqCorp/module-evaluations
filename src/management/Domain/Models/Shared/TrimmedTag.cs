using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// Immutable tag decorator that trims whitespace from the original text.
/// </summary>
public sealed record TrimmedTag : ITag
{
    private readonly ITag _original;

    /// <summary>
    /// Initializes the decorator with the original tag source.
    /// </summary>
    /// <param name="original">Tag source to trim.</param>
    public TrimmedTag(ITag original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
