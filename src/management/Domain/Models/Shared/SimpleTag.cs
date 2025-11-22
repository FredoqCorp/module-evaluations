using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// JSON-backed tag that reads the tag text from the underlying JSON value.
/// </summary>
public sealed record SimpleTag : ITag
{
    private readonly string _text;

    /// <summary>
    /// Initializes the tag reader from the provided JSON element.
    /// </summary>
    /// <param name="element">JSON element that contains the tag text.</param>
    public SimpleTag(JsonElement element) : this(element.String())
    {
    }

    /// <summary>
    /// Initializes the tag reader from the provided text.
    /// </summary>
    /// <param name="text">The text of the tag.</param>
    public SimpleTag(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        _text = text;
    }

    /// <inheritdoc />
    public string Text() => _text;
}
