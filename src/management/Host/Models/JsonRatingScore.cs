using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed rating score that reads the numeric score field.
/// </summary>
internal sealed record JsonRatingScore : IRatingScore
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes the rating score from the provided option node.
    /// </summary>
    /// <param name="node">JSON element that contains the score field.</param>
    public JsonRatingScore(JsonElement node)
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
