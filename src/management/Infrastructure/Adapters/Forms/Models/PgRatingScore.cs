using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms.Models;

/// <summary>
/// JSON-backed rating score extracted from PostgreSQL payload nodes.
/// </summary>
internal sealed record PgRatingScore : IRatingScore
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes from the node describing a rating option.
    /// </summary>
    /// <param name="node">JSON element that contains the score field.</param>
    public PgRatingScore(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public int Value()
    {
        if (!_node.TryGetProperty("score", out var value))
        {
            throw new InvalidOperationException("Rating option requires 'score'");
        }

        if (value.ValueKind is not JsonValueKind.Number)
        {
            throw new InvalidOperationException("Rating option score must be a number");
        }

        return value.GetInt32();
    }
}
