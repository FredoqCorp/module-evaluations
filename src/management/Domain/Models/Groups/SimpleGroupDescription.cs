using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Represents a simple group description extracted from JSON.
/// </summary>
public sealed record SimpleGroupDescription : IGroupDescription
{
    private readonly string _text;

    /// <summary>
    /// Creates a JSON-backed group description.
    /// </summary>
    /// <param name="node">JSON node containing the description field.</param>
    public SimpleGroupDescription(JsonElement node) : this(node.String("description"))
    {
    }

    /// <summary>
    /// Creates a simple group description from a string.
    /// </summary>
    /// <param name="text">Description value.</param>
    public SimpleGroupDescription(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        _text = text;
    }

    /// <inheritdoc />
    public string Text() => _text;
}
