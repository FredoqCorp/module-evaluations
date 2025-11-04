using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;
using CascVel.Modules.Evaluations.Management.Host.Models.Criteria;
using CascVel.Modules.Evaluations.Management.Host.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Groups;

/// <summary>
/// Represents a group backed by JSON.
/// </summary>
internal sealed record JsonNewAverageGroup : IGroup
{
    private readonly JsonElement _node;
    private readonly GroupId _identifier;

    /// <summary>
    /// Creates a JSON-backed group.
    /// </summary>
    /// <param name="node">JSON element describing the group.</param>
    public JsonNewAverageGroup(JsonElement node)
    {
        _node = node;
        _identifier = new GroupId();
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("id", _identifier.Value);
        media.With("title", new ValidGroupTitle(new TrimmedGroupTitle(new JsonGroupTitle(_node))).Text());
        media.With("description", new ValidGroupDescription(new TrimmedGroupDescription(new JsonGroupDescription(_node))).Text());
        media.With("orderIndex", new ValidOrderIndex(new JsonOrderIndex(_node)).Value());
        new JsonCriteria(_node, CalculationType.Average).Print(media);
        new JsonGroups(_node, CalculationType.Average).Print(media);
        return media;
    }
}
