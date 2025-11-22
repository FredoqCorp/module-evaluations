using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Represents a group that aggregates criteria using average calculation.
/// </summary>
public sealed record SimpleAverageGroup : IGroup
{
    private readonly GroupId _id;
    private readonly IGroupTitle _title;
    private readonly IGroupDescription _description;
    private readonly IOrderIndex _orderIndex;
    private readonly ICriteria _criteria;
    private readonly IGroups _groups;

    /// <summary>
    /// Creates a JSON-backed group.
    /// </summary>
    /// <param name="node">JSON element describing the group.</param>
    public SimpleAverageGroup(JsonElement node) : this(
        new GroupId(node),
        new ValidGroupTitle(new TrimmedGroupTitle(new SimpleGroupTitle(node))),
        new ValidGroupDescription(new TrimmedGroupDescription(new SimpleGroupDescription(node))),
        new ValidOrderIndex(new SimpleOrderIndex(node)),
        new SimpleCriteria(node, static (_, element) => new SimpleAverageCriterion(element), CalculationType.Average),
        new SimpleGroups(node, static (_, element) => new SimpleAverageGroup(element), CalculationType.Average))
    {
    }

    /// <summary>
    /// Creates a group from its components.
    /// </summary>
    public SimpleAverageGroup(
        GroupId id,
        IGroupTitle title,
        IGroupDescription description,
        IOrderIndex orderIndex,
        ICriteria criteria,
        IGroups groups)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(orderIndex);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);
        _id = id;
        _title = title;
        _description = description;
        _orderIndex = orderIndex;
        _criteria = criteria;
        _groups = groups;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("id", _id.Value());
        media.With("title", _title.Text());
        media.With("description", _description.Text());
        media.With("orderIndex", _orderIndex.Value());
        await _criteria.Print(media);
        await _groups.Print(media);
        return media;
    }
}
