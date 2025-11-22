using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// JSON-backed order index that extracts the integer order field.
/// </summary>
public sealed class SimpleOrderIndex : IOrderIndex
{    private readonly int _order;

    /// <summary>
    /// Initializes the order index reader from the JSON node.
    /// </summary>
    /// <param name="node">JSON element that provides the order property.</param>
    public SimpleOrderIndex(JsonElement node) : this(node.Number("order"))
    {
    }

    /// <summary>
    /// Initializes the order index with a specific order value.
    /// </summary>
    /// <param name="order">Order index value.</param>
    public SimpleOrderIndex(int order)
    {
        _order = order;
    }

    /// <inheritdoc />
    public int Value() => _order;
}
