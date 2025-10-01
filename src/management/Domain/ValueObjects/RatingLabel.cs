namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores a mandatory label for a rating option.
/// </summary>
public readonly record struct RatingLabel
{
    /// <summary>
    /// Creates a label while ensuring that it is not blank.
    /// </summary>
    /// <param name="value">Raw label text provided by the designer.</param>
    public RatingLabel(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value.Trim();
    }

    /// <summary>
    /// Normalized label text.
    /// </summary>
    public string Value { get; }
}
