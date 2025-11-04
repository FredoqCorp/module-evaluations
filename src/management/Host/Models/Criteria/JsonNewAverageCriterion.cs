using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;
using CascVel.Modules.Evaluations.Management.Host.Models.Ratings;
using CascVel.Modules.Evaluations.Management.Host.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Criteria;

/// <summary>
/// Represents a criterion backed by JSON.
/// </summary>
internal sealed record JsonNewAverageCriterion : ICriterion
{
    private readonly JsonElement _node;
    private readonly CriterionId _identifier;

    /// <summary>
    /// Creates a JSON-backed criterion.
    /// </summary>
    /// <param name="node">JSON element describing the criterion.</param>
    public JsonNewAverageCriterion(JsonElement node)
    {
        _node = node;
        _identifier = new CriterionId();

    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("id", _identifier.Value);
        media.With("title", new ValidCriterionTitle(new TrimmedCriterionTitle(new JsonCriterionTitle(_node))).Text());
        media.With("text", new ValidCriterionText(new TrimmedCriterionText(new JsonCriterionText(_node))).Text());
        media.With("orderIndex", new ValidOrderIndex(new JsonOrderIndex(_node)).Value());
        new JsonRatingOptions(_node).Print(media);
        return media;
    }
}
