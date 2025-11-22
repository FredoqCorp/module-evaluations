using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Ratings;

/// <summary>
/// JSON-backed rating score that reads the numeric score field.
/// </summary>
public sealed record SimpleRatingScore : IRatingScore
{
    private readonly int _score;

    /// <summary>
    /// Initializes the rating score from the provided option node.
    /// </summary>
    /// <param name="node">JSON element that contains the score field.</param>
    public SimpleRatingScore(JsonElement node) : this(node.Number("score"))
    {
    }

    /// <summary>
    /// Initializes the rating score with the provided numeric value.
    /// </summary>
    /// <param name="score">The numeric score value.</param>
    public SimpleRatingScore(int score)
    {
        _score = score;
    }

    

    /// <inheritdoc />
    public int Value() => _score;
}
