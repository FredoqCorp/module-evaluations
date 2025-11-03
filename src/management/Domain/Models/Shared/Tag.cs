using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// Immutable value object that stores a single keyword preserving original text and a case-insensitive fingerprint.
/// </summary>
public record Tag : ITag
{
    private readonly string _text;
    /// <summary>
    /// Creates a tag from raw keyword text ensuring it is not blank.
    /// </summary>
    /// <param name="text">Keyword text provided by the caller.</param>
    public Tag(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        _text = text;
    }

    /// <summary>
    /// Returns the original keyword text.
    /// </summary>
    /// <returns>Original keyword text.</returns>
    public string Text() => _text;
}
