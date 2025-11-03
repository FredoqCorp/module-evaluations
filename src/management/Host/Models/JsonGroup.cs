using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents a group backed by JSON.
/// </summary>
internal sealed class JsonGroup : IGroup
{
    private readonly JsonElement _node;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a JSON-backed group.
    /// </summary>
    /// <param name="node">JSON element describing the group.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonGroup(JsonElement node, CalculationType calculation)
    {
        _node = node;
        _calculation = calculation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        var identifier = Identifier(_node);

        media.WithObject("group", inner =>
        {
            inner.With("id", identifier.Value);

            inner.With("title", JsonFormNodes.Text(_node, "title"));
            inner.With("description", JsonFormNodes.Description(_node));
            inner.With("orderIndex", JsonFormNodes.Order(_node));

            var weight = JsonFormNodes.Weight(_node, _calculation, "group");
            if (weight.HasValue)
            {
                inner.With("weightBasisPoints", weight.Value);
            }
        });

        var criteria = new JsonCriteria(_node, _calculation);
        criteria.Print(media, formId, identifier);

        var groups = new JsonGroups(_node, _calculation);
        groups.Print(media, formId, identifier);
        return media;
    }

    public static GroupId Identifier(JsonElement node)
    {
        if (node.TryGetProperty("id", out var value) && value.ValueKind == JsonValueKind.String)
        {
            return new GroupId(value.GetGuid());
        }

        return new GroupId();
    }
}
