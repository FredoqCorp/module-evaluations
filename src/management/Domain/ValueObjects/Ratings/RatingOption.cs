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
    /// <param name="score">Numeric score value object.</param>
    /// <param name="label">Label value object.</param>
    /// <param name="annotation">Annotation value object.</param>
    public RatingOption(RatingScore score, RatingLabel label, RatingAnnotation annotation)
    {
        _score = score;
        _label = label;
        _annotation = annotation;
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

    /// <summary>
    /// Calculates the contribution of this option to the total form score.
    /// </summary>
    /// <returns>A contribution of zero because the option is not selected.</returns>
    public IRatingContribution Contribution()
    {
        return new RatingContribution(decimal.Zero, 0);
    }
}
