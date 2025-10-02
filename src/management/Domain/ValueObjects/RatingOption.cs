using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable composition of score, label, and annotation for use inside rating scales.
/// </summary>
public sealed record RatingOption : IRatingOption
{
    private readonly RatingScore _score;
    private readonly RatingLabel _label;
    private readonly RatingAnnotation _annotation;

    /// <summary>
    /// Creates a rating option from the provided components without accepting null.
    /// </summary>
    /// <param name="scoreValue">Numeric score value object.</param>
    /// <param name="labelValue">Label value object.</param>
    /// <param name="annotationValue">Annotation value object.</param>
    public RatingOption(RatingScore scoreValue, RatingLabel labelValue, RatingAnnotation annotationValue)
    {
        _score = scoreValue;
        _label = labelValue;
        _annotation = annotationValue;
    }

    /// <summary>
    /// Determines if this rating option matches the given score.
    /// </summary>
    /// <param name="score">The score to compare against.</param>
    /// <returns>True if this option has the given score.</returns>
    public bool Matches(RatingScore score)
    {
        return _score.Equals(score);
    }
}
