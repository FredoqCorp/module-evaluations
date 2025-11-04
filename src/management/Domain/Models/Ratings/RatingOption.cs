using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

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

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.With("score", _score.Value);
        media.With("label", _label.Value);
        media.With("annotation", _annotation.Text);
        return media;
    }
}
