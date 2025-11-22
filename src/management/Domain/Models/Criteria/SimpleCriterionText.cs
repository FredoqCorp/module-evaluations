using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Represents a simple criterion text.
/// </summary>
public sealed record SimpleCriterionText : ICriterionText
{
    private readonly string _text;

    /// <summary>
    /// Creates a JSON-backed criterion text.
    /// </summary>
    /// <param name="node">JSON node containing the text field.</param>
    public SimpleCriterionText(JsonElement node) : this(node.String("text"))
    {
    }

    /// <summary>
    /// Creates a simple criterion text from a string.
    /// </summary>
    public SimpleCriterionText(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        _text = text;
    }


    /// <inheritdoc />
    public string Text() => _text;
}
