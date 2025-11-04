using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable decorator that validates the annotation text for rating options.
/// </summary>
public sealed class ValidRatingAnnotation : IRatingAnnotation
{
    private readonly IRatingAnnotation _original;

    /// <summary>
    /// Initializes the decorator with the original annotation source.
    /// </summary>
    /// <param name="original">Annotation source to validate.</param>
    public ValidRatingAnnotation(IRatingAnnotation original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var text = _original.Text();
        ArgumentNullException.ThrowIfNull(text);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 1000);
        return text;
    }
}
