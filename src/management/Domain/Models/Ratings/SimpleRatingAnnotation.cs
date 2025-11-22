using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Represents a simple rating annotation with text.
/// </summary>
public sealed record SimpleRatingAnnotation : IRatingAnnotation
{
    private readonly string _annotation;

    /// <summary>
    /// Initializes the rating annotation from the provided option node.
    /// </summary>
    /// <param name="node">JSON element that may contain the annotation field.</param>
    public SimpleRatingAnnotation(JsonElement node) : this(node.String("annotation"))
    {
    }

    /// <summary>
    /// Initializes an empty rating annotation.
    /// </summary>
    public SimpleRatingAnnotation()
        : this(string.Empty)
    {
    }

    /// <summary>
    /// Initializes the rating annotation with the provided text.
    /// </summary>
    /// <param name="annotation">The annotation text.</param>
    public SimpleRatingAnnotation(string annotation)
    {
        _annotation = annotation;
    }

    /// <inheritdoc />
    public string Text() => _annotation;
}
