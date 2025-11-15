using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Models;

/// <summary>
/// JSON-backed rating option composed of score, label, and annotation from PostgreSQL payloads.
/// </summary>
internal sealed record PgRatingOption : IRatingOption
{
    private readonly int _score;
    private readonly string _label;
    private readonly string _annotation;

    /// <summary>
    /// Initializes the option from the JSON node provided by PostgreSQL.
    /// </summary>
    /// <param name="node">JSON element describing the rating option.</param>
    public PgRatingOption(JsonElement node)
    {
        _score = new ValidRatingScore(new PgRatingScore(node)).Value();
        _label = new ValidRatingLabel(new TrimmedRatingLabel(new PgRatingLabel(node))).Text();
        _annotation = new ValidRatingAnnotation(new TrimmedRatingAnnotation(new PgRatingAnnotation(node))).Text();
    }

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
