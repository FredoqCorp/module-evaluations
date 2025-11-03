using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents a criterion backed by JSON.
/// </summary>
internal sealed class JsonCriterion : ICriterion
{
    private static readonly JsonSerializerOptions RatingSerialization = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly JsonElement _node;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a JSON-backed criterion.
    /// </summary>
    /// <param name="node">JSON element describing the criterion.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonCriterion(JsonElement node, CalculationType calculation)
    {
        _node = node;
        _calculation = calculation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, Guid formId, Guid? groupId)
    {
        ArgumentNullException.ThrowIfNull(media);

        var identifier = JsonFormNodes.Identifier(_node);
        var weight = JsonFormNodes.Weight(_node, _calculation, "criterion");
        var ratings = JsonFormNodes.Ratings(_node, RatingSerialization);

        media.WithObject("criterion", inner =>
        {
            inner.With("id", identifier);
            if (groupId.HasValue)
            {
                inner.With("groupId", groupId.Value);
            }
            else
            {
                inner.With("formId", formId);
            }

            inner.With("title", JsonFormNodes.Text(_node, "title"));
            inner.With("text", JsonFormNodes.Text(_node, "text"));
            inner.With("orderIndex", JsonFormNodes.Order(_node));
            inner.With("ratingOptions", ratings);

            if (weight.HasValue)
            {
                inner.With("weightBasisPoints", weight.Value);
            }
        });
        return media;
    }
}
