using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Represents a simple weighted criterion.
/// </summary>
public sealed record SimpleWeightedCriterion : ICriterion
{
    private readonly ICriterion _original;
    private readonly IWeight _weight;

    /// <summary>
    /// Creates a JSON-backed criterion.
    /// </summary>
    /// <param name="node">JSON element describing the criterion.</param>
    /// <param name="original">Original criterion to copy data from.</param>
    public SimpleWeightedCriterion(JsonElement node, ICriterion original) : this(original, new SimpleWeight(node))
    {
    }

    /// <summary>
    /// Creates a weighted criterion from its components.
    /// </summary>
    public SimpleWeightedCriterion(ICriterion original, IWeight weight)
    {
        ArgumentNullException.ThrowIfNull(original);
        ArgumentNullException.ThrowIfNull(weight);
        _original = original;
        _weight = weight;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        await _original.Print(media);
        media.With("weightBasisPoints", _weight.BasisPoints());
        return media;
    }
}
