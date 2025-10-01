namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores an optional annotation text for rating guidance.
/// </summary>
public readonly record struct RatingAnnotation
{
    /// <summary>
    /// Creates an annotation, accepting empty strings as a signal of absence.
    /// </summary>
    /// <param name="value">Raw annotation text supplied by the designer.</param>
    public RatingAnnotation(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Text = value.Trim();
    }

    /// <summary>
    /// Normalized annotation text that may be empty when no annotation is provided.
    /// </summary>
    public string Text { get; }
}
