using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed rating annotation that reads the optional annotation field.
/// </summary>
public sealed record JsonRatingAnnotation : IRatingAnnotation
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes the rating annotation from the provided option node.
    /// </summary>
    /// <param name="node">JSON element that may contain the annotation field.</param>
    public JsonRatingAnnotation(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_node.TryGetProperty("annotation", out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return string.Empty;
        }

        if (value.ValueKind is not JsonValueKind.String)
        {
            throw new InvalidOperationException("Rating option annotation must be a string");
        }

        return value.GetString() ?? string.Empty;
    }
}
