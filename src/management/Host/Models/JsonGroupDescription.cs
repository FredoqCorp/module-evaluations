using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed group description that reads the description token from a JSON node.
/// </summary>
internal sealed record JsonGroupDescription : IGroupDescription
{
    private readonly JsonElement _node;

    /// <summary>
    /// Creates a JSON-backed group description.
    /// </summary>
    /// <param name="node">JSON node containing the description field.</param>
    public JsonGroupDescription(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_node.TryGetProperty("description", out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return string.Empty;
        }

        if (value.ValueKind is not JsonValueKind.String)
        {
            throw new InvalidOperationException("Group description must be a string");
        }

        return value.GetString() ?? throw new InvalidOperationException("Group description must be a string");
    }
}
