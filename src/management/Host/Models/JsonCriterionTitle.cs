using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed criterion title that reads the title token from a JSON node.
/// </summary>
public sealed record JsonCriterionTitle : ICriterionTitle
{
    private readonly JsonElement _node;

    /// <summary>
    /// Creates a JSON-backed criterion title.
    /// </summary>
    /// <param name="node">JSON node containing the title field.</param>
    public JsonCriterionTitle(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_node.TryGetProperty("title", out var value))
        {
            throw new InvalidOperationException($"Missing 'title' property");
        }
        return value.GetString() ?? throw new InvalidOperationException($"Property 'title' must be a string");
    }
}
