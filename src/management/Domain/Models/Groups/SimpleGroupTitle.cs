using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Represents a simple group title extracted from JSON.
/// </summary>
public sealed record SimpleGroupTitle : IGroupTitle
{
    private readonly string _text;

    /// <summary>
    /// Creates a JSON-backed group title.
    /// </summary>
    /// <param name="node">JSON node containing the title field.</param>
    public SimpleGroupTitle(JsonElement node) : this(node.String("title"))
    {
    }

    /// <summary>
    /// Creates a simple group title from a string.
    /// </summary>
    /// <param name="text">Title value.</param>
    public SimpleGroupTitle(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        _text = text;
    }

    /// <inheritdoc />
    public string Text() => _text;
}
