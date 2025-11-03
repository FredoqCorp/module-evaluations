using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed form name that reads the name from the underlying JSON element.
/// </summary>
public record JsonFormName : IFormName
{
    private readonly JsonElement _element;

    /// <summary>
    /// Initializes the form name from the provided JSON element.
    /// </summary>
    /// <param name="element">JSON element that contains the form name.</param>
    public JsonFormName(JsonElement element)
    {
        _element = element;
    }


    /// <inheritdoc />
    public string Text()
    {
        if (!_element.TryGetProperty("name", out var value))
        {
            throw new InvalidOperationException($"Missing 'name' property");
        }

        return value.GetString() ?? throw new InvalidOperationException($"Property 'name' must be a string");
    }
}
