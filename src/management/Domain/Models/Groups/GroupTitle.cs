namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Short human-readable title for a criterion group, limited to 100 characters.
/// </summary>
public readonly record struct GroupTitle
{
    /// <summary>
    /// Creates group title ensuring it is non-empty and within the 100 character limit.
    /// </summary>
    /// <param name="text">Non-empty title not exceeding 100 characters.</param>
    /// <exception cref="ArgumentException">Thrown when title is empty, whitespace only, or exceeds 100 characters.</exception>
    public GroupTitle(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 100);

        Text = text;
    }

    /// <summary>
    /// Underlying string value representing the group title.
    /// </summary>
    public string Text { get; init; }
}
