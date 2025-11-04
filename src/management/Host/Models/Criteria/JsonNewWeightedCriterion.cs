using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Host.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Criteria;

/// <summary>
/// Represents a criterion backed by JSON.
/// </summary>
internal sealed record JsonNewWeightedCriterion : ICriterion
{
    private readonly JsonElement _node;
    private readonly ICriterion _original;

    /// <summary>
    /// Creates a JSON-backed criterion.
    /// </summary>
    /// <param name="node">JSON element describing the criterion.</param>
    /// <param name="original">Original criterion to copy data from.</param>
    public JsonNewWeightedCriterion(JsonElement node, ICriterion original)
    {
        _node = node;
        _original = original;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        _original.Print(media);
        media.With("weightBasisPoints", new JsonWeight(_node).BasisPoints());
        return media;
    }
}
