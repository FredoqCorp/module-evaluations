namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Optional detailed description for a criterion group, limited to 1000 characters.
/// </summary>
public readonly record struct GroupDescription
{
    /// <summary>
    /// Creates group description ensuring it does not exceed the 1000 character limit.
    /// </summary>
    /// <param name="text">Description text not exceeding 1000 characters.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when text exceeds 1000 characters.</exception>
    public GroupDescription(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 1000);

        Text = text;
    }

    /// <summary>
    /// Underlying string value representing the group description.
    /// </summary>
    public string Text { get; init; }
}
