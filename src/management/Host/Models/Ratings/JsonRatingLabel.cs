using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Ratings;

/// <summary>
/// JSON-backed rating label that reads the label field from an option node.
/// </summary>
internal sealed record JsonRatingLabel : IRatingLabel
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes the rating label from the provided option node.
    /// </summary>
    /// <param name="node">JSON element that contains the label field.</param>
    public JsonRatingLabel(JsonElement node)
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
