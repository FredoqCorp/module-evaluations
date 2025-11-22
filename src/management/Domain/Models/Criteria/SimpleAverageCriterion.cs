using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Represents a criterion.
/// </summary>
public sealed record SimpleAverageCriterion : ICriterion
{
    private readonly ICriterionId _id;
    private readonly ICriterionText _text;
    private readonly ICriterionTitle _title;
    private readonly IOrderIndex _orderIndex;
    private readonly IRatingOptions _ratingOptions;

    /// <summary>
    /// Creates a JSON-backed criterion.
    /// </summary>
    /// <param name="node">JSON element describing the criterion.</param>
    public SimpleAverageCriterion(JsonElement node) : this(
        new SimpleCriterionId(node),
        new ValidCriterionText(new TrimmedCriterionText(new SimpleCriterionText(node))),
        new ValidCriterionTitle(new TrimmedCriterionTitle(new SimpleCriterionTitle(node))),
        new ValidOrderIndex(new SimpleOrderIndex(node)),
        new SimpleRatingOptions(node))
    {
    }

    /// <summary>
    /// Creates a criterion from its components.
    /// </summary>
    public SimpleAverageCriterion(ICriterionId id, ICriterionText text, ICriterionTitle title, IOrderIndex orderIndex, IRatingOptions ratingOptions)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(orderIndex);
        ArgumentNullException.ThrowIfNull(ratingOptions);
        _id = id;
        _text = text;
        _title = title;
        _orderIndex = orderIndex;
        _ratingOptions = ratingOptions;
    }
    

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("id", _id.Value());
        media.With("title", _title.Text());
        media.With("text", _text.Text());
        media.With("orderIndex", _orderIndex.Value());
        await _ratingOptions.Print(media);
        return media;
    }
}
