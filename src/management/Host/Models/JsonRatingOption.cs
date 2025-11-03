using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed rating option composed of score, label, and annotation.
/// </summary>
public sealed class JsonRatingOption : IRatingOption
{
    private readonly int _score;
    private readonly string _label;
    private readonly string _annotation;

    /// <summary>
    /// Initializes the rating option from the JSON node.
    /// </summary>
    /// <param name="node">JSON element describing the rating option.</param>
    public JsonRatingOption(JsonElement node)
    {
        _score = new ValidRatingScore(new JsonRatingScore(node)).Value();
        _label = new ValidRatingLabel(new TrimmedRatingLabel(new JsonRatingLabel(node))).Text();
        _annotation = new ValidRatingAnnotation(new TrimmedRatingAnnotation(new JsonRatingAnnotation(node))).Text();
    }

    /// <summary>
    /// Resolved numeric score used for ordering.
    /// </summary>
    public int Score => _score;

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.With("score", _score);
        media.With("label", _label);
        media.With("annotation", _annotation);

        return media;
    }
}
