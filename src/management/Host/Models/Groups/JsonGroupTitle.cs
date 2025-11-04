using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Groups;

/// <summary>
/// JSON-backed group title that reads the title token from a JSON node.
/// </summary>
internal sealed record JsonGroupTitle : IGroupTitle
{
    private readonly JsonElement _node;

    /// <summary>
    /// Creates a JSON-backed group title.
    /// </summary>
    /// <param name="node">JSON node containing the title field.</param>
    public JsonGroupTitle(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_node.TryGetProperty("title", out var value))
        {
            throw new InvalidOperationException("Group requires 'title' property");
        }

        return value.GetString() ?? throw new InvalidOperationException("Group title must be a string");
    }
}
