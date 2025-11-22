using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Host.Models.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Rating option.
/// </summary>
public sealed record SimpleRatingOption : IRatingOption
{
    private readonly IRatingScore _score;
    private readonly IRatingLabel _label;
    private readonly IRatingAnnotation _annotation;

    /// <summary>
    /// Initializes the rating option from the JSON node.
    /// </summary>
    /// <param name="node">JSON element describing the rating option.</param>
    public SimpleRatingOption(JsonElement node) : this(
        new ValidRatingScore(new SimpleRatingScore(node)),
        new ValidRatingLabel(new TrimmedRatingLabel(new SimpleRatingLabel(node))),
        new ValidRatingAnnotation(new TrimmedRatingAnnotation(new SimpleRatingAnnotation(node))))
    {
    }

    /// <summary>
    /// Initializes the rating option with provided components.
    /// </summary>
    /// <param name="score">The rating score component.</param>
    /// <param name="label">The rating label component.</param>
    /// <param name="annotation">The rating annotation component.</param>
    public SimpleRatingOption(IRatingScore score, IRatingLabel label, IRatingAnnotation annotation)
    {
        ArgumentNullException.ThrowIfNull(score);
        ArgumentNullException.ThrowIfNull(label);
        ArgumentNullException.ThrowIfNull(annotation);
        _score = score;
        _label = label;
        _annotation = annotation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("score", _score.Value());
        media.With("label", _label.Text());
        media.With("annotation", _annotation.Text());
        return media;
    }
}
