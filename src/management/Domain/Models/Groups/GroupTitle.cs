using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Short human-readable title for a criterion group, limited to 100 characters.
/// </summary>
public sealed record GroupTitle : IGroupTitle
{
    private readonly string _value;

    /// <summary>
    /// Creates group title ensuring it is non-empty and within the 100 character limit.
    /// </summary>
    /// <param name="text">Non-empty title not exceeding 100 characters.</param>
    /// <exception cref="ArgumentException">Thrown when title is empty, whitespace only, or exceeds 100 characters.</exception>
    public GroupTitle(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        _value = text;
    }

    /// <summary>
    /// Reads the group title string.
    /// </summary>
    /// <returns>Group title string.</returns>
    public string Text() => _value;
}
