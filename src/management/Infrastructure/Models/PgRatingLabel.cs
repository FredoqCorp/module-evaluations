using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Models;

/// <summary>
/// JSON-backed rating label extracted from PostgreSQL payload nodes.
/// </summary>
internal sealed record PgRatingLabel : IRatingLabel
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes from the node describing a rating option.
    /// </summary>
    /// <param name="node">JSON element that contains the label field.</param>
    public PgRatingLabel(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_node.TryGetProperty("label", out var value))
        {
            throw new InvalidOperationException("Rating option requires 'label'");
        }

        var text = value.GetString();
        return text ?? throw new InvalidOperationException("Rating option label must be a string");
    }
}
