using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Forms;

/// <summary>
/// JSON-backed form code that reads the code token from the underlying JSON element.
/// </summary>
internal sealed record JsonFormCode : IFormCode
{
    private readonly JsonElement _element;

    /// <summary>
    /// Initializes the form code from the provided JSON element.
    /// </summary>
    /// <param name="element">JSON element that contains the form code.</param>
    public JsonFormCode(JsonElement element)
    {
        _element = element;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_element.TryGetProperty("code", out var value))
        {
            throw new InvalidOperationException("Missing 'code' property");
        }

        if (value.ValueKind != JsonValueKind.String)
        {
            throw new InvalidOperationException("Property 'code' must be a string");
        }

        var text = value.GetString();
        return text ?? throw new InvalidOperationException("Property 'code' must be a string");
    }
}
