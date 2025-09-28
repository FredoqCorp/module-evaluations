namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores a single keyword preserving original text and a case-insensitive fingerprint.
/// </summary>
public readonly record struct Tag
{
    /// <summary>
    /// Creates a tag from raw keyword text ensuring it is not blank.
    /// </summary>
    /// <param name="text">Keyword text provided by the caller.</param>
    public Tag(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);

        Text = text.Trim();
    }

    /// <summary>
    /// Returns the original keyword text.
    /// </summary>
    /// <returns>Original keyword text.</returns>
    public string Text { get; init; }
}
