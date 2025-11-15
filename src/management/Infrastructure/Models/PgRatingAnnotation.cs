using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Models;

/// <summary>
/// JSON-backed rating annotation extracted from PostgreSQL payload nodes.
/// </summary>
internal sealed record PgRatingAnnotation : IRatingAnnotation
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes from the node describing a rating option.
    /// </summary>
    /// <param name="node">JSON element that may contain the annotation field.</param>
    public PgRatingAnnotation(JsonElement node)
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
