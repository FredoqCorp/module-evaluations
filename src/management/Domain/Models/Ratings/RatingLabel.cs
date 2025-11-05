using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable value object that stores a mandatory label for a rating option.
/// </summary>
public sealed record RatingLabel : IRatingLabel
{
    private readonly string _value;

    /// <summary>
    /// Creates a label while ensuring that it is not blank.
    /// </summary>
    /// <param name="value">Raw label text provided by the designer.</param>
    public RatingLabel(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _value = value;
    }

    /// <summary>
    /// Reads the label text.
    /// </summary>
    /// <returns>Rating label string.</returns>
    public string Text() => _value;
}
