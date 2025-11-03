using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed criterion text that extracts the textual body from a JSON element.
/// </summary>
internal sealed record JsonCriterionText : ICriterionText
{
    private readonly JsonElement _node;

    /// <summary>
    /// Creates a JSON-backed criterion text.
    /// </summary>
    /// <param name="node">JSON node containing the text field.</param>
    public JsonCriterionText(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_node.TryGetProperty("text", out var value))
        {
            throw new InvalidOperationException($"Missing 'text' property");
        }
        return value.GetString() ?? throw new InvalidOperationException($"Property 'text' must be a string");
    }
}
