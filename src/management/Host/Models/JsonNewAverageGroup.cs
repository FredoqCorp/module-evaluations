using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

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
        media.With("title", JsonFormNodes.Text(_node, "title"));
        media.With("description", JsonFormNodes.Description(_node));
        media.With("orderIndex", JsonFormNodes.Order(_node));

        var criteria = new JsonCriteria(_node, CalculationType.Average);
        criteria.Print(media);

        //var groups = new JsonGroups(_node, CalculationType.Average);
        //groups.Print(media);
        return media;
    }
}
