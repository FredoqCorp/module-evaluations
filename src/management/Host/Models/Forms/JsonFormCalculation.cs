using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Forms;

/// <summary>
/// JSON-backed form calculation that reads the calculation type from the underlying JSON element.
/// </summary>
internal sealed record JsonFormCalculation : IFormCalculation
{
    private readonly JsonElement _element;
    
    /// <summary>
    /// Initializes the form calculation from the provided JSON element.
    /// </summary>
    /// <param name="element"></param>
    public JsonFormCalculation(JsonElement element)
    {
        _element = element;
    }

    /// <inheritdoc />
    public CalculationType Type()
    {
        var token = Text(_element, "calculation");
        if (string.Equals(token, "average", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.Average;
        }

        if (string.Equals(token, "weighted", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.WeightedAverage;
        }

        throw new InvalidOperationException("Unsupported calculation token");
    }

    private static string Text(JsonElement node, string name)
    {
        if (!node.TryGetProperty(name, out var value))
        {
            throw new InvalidOperationException($"Missing '{name}' property");
        }

        var text = value.GetString() ?? throw new InvalidOperationException($"Property '{name}' must be a string");
        return text.Trim();
    }
}
