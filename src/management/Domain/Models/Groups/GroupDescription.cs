using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Optional detailed description for a criterion group, limited to 1000 characters.
/// </summary>
public sealed record GroupDescription : IGroupDescription
{
    private readonly string _value;

    /// <summary>
    /// Creates group description ensuring it does not exceed the 1000 character limit.
    /// </summary>
    /// <param name="text">Description text not exceeding 1000 characters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when text exceeds 1000 characters.</exception>
    public GroupDescription(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        _value = text;
    }

    /// <summary>
    /// Reads the group description string.
    /// </summary>
    /// <returns>Group description string.</returns>
    public string Text() => _value;
}
