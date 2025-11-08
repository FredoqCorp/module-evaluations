using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable value object that stores an optional annotation text for rating guidance.
/// </summary>
public sealed record RatingAnnotation : IRatingAnnotation
{
    private readonly string _value;

    /// <summary>
    /// Creates an annotation, accepting empty strings as a signal of absence.
    /// </summary>
    /// <param name="value">Raw annotation text supplied by the designer.</param>
    public RatingAnnotation(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _value = value;
    }

    /// <summary>
    /// Reads the annotation text.
    /// </summary>
    /// <returns>Rating annotation string.</returns>
    public string Text() => _value;
}
