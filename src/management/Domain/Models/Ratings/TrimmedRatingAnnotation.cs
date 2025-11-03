using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable decorator that trims the annotation text for a rating option.
/// </summary>
public sealed class TrimmedRatingAnnotation : IRatingAnnotation
{
    private readonly IRatingAnnotation _original;

    /// <summary>
    /// Initializes the decorator with the original annotation source.
    /// </summary>
    /// <param name="original">Annotation source to trim.</param>
    public TrimmedRatingAnnotation(IRatingAnnotation original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
