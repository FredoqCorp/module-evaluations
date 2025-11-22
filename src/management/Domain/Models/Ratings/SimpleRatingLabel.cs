using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Represents a simple rating label with text.
/// </summary>
public sealed record SimpleRatingLabel : IRatingLabel
{
    private readonly string _label;

    /// <summary>
    /// Initializes the rating label from the provided option node.
    /// </summary>
    /// <param name="node">JSON element that contains the label field.</param>
    public SimpleRatingLabel(JsonElement node) : this(node.String("label"))
    {
    }

    /// <summary>
    /// Initializes the rating label with the provided text.
    /// </summary>
    /// <param name="label">The label text.</param>
    public SimpleRatingLabel(string label)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        _label = label;
    }

    /// <inheritdoc />
    public string Text() => _label;
}
