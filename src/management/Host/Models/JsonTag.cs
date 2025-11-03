using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed tag that reads the tag text from the underlying JSON value.
/// </summary>
internal sealed record JsonTag : ITag
{
    private readonly JsonElement _element;

    /// <summary>
    /// Initializes the tag reader from the provided JSON element.
    /// </summary>
    /// <param name="element">JSON element that contains the tag text.</param>
    public JsonTag(JsonElement element)
    {
        _element = element;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (_element.ValueKind != JsonValueKind.String)
        {
            throw new InvalidOperationException("Tag value must be a string");
        }

        var text = _element.GetString();
        return text ?? throw new InvalidOperationException("Tag value must be a string");
    }
}
