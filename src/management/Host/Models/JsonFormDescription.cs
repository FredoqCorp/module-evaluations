using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed form description that reads the description from the underlying JSON element.
/// </summary>
public sealed class JsonFormDescription : IFormDescription
{
    private readonly JsonElement _element;

    /// <summary>
    /// Initializes the form description from the provided JSON element.
    /// </summary>
    /// <param name="element">JSON element that contains the form description.</param>
    public JsonFormDescription(JsonElement element)
    {
        _element = element;
    }

    /// <inheritdoc />
    public string Text()
    {
        if (!_element.TryGetProperty("description", out var value))
        {
            throw new InvalidOperationException("Missing 'description' property");
        }

        if (value.ValueKind != JsonValueKind.String)
        {
            throw new InvalidOperationException("Property 'description' must be a string");
        }

        var text = value.GetString();
        return text ?? throw new InvalidOperationException("Property 'description' must be a string");
    }
}
