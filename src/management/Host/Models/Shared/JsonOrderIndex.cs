using System;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Shared;

/// <summary>
/// JSON-backed order index that extracts the integer order field.
/// </summary>
internal sealed class JsonOrderIndex : IOrderIndex
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes the order index reader from the JSON node.
    /// </summary>
    /// <param name="node">JSON element that provides the order property.</param>
    public JsonOrderIndex(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public int Value()
    {
        if (!_node.TryGetProperty("order", out var value))
        {
            throw new InvalidOperationException("Criterion requires 'order' property");
        }

        if (value.ValueKind is not JsonValueKind.Number)
        {
            throw new InvalidOperationException("Criterion order must be a number");
        }

        return value.GetInt32();
    }
}
