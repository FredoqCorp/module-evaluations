using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents a criterion backed by JSON.
/// </summary>
internal sealed class JsonNewCriterion : ICriterion
{
    private static readonly JsonSerializerOptions RatingSerialization = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly JsonElement _node;
    private readonly CalculationType _calculation;
    private readonly CriterionId _identifier;

    /// <summary>
    /// Creates a JSON-backed criterion.
    /// </summary>
    /// <param name="node">JSON element describing the criterion.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonNewCriterion(JsonElement node, CalculationType calculation)
    {
        _node = node;
        _calculation = calculation;
        _identifier = new CriterionId();

    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        var weight = JsonFormNodes.Weight(_node, _calculation, "criterion");
        var ratings = JsonFormNodes.Ratings(_node, RatingSerialization);

        media.With("id", _identifier.Value);

        media.With("title", new ValidCriterionTitle(new TrimmedCriterionTitle(new JsonCriterionTitle(_node))).Text());
        media.With("text", new ValidCriterionText(new TrimmedCriterionText(new JsonCriterionText(_node))).Text());
        media.With("orderIndex", JsonFormNodes.Order(_node));
        media.With("ratingOptions", ratings);

        if (weight.HasValue)
        {
            media.With("weightBasisPoints", weight.Value);
        }
        return media;
    }
}
