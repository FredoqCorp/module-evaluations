using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents a groups collection backed by JSON.
/// </summary>
internal sealed class JsonGroups : IGroups
{
    private readonly JsonElement _container;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a JSON-backed groups collection.
    /// </summary>
    /// <param name="document">JSON document containing the groups array.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonGroups(JsonDocument document, CalculationType calculation)
    {
        _container = document.RootElement.GetProperty("root");
        _calculation = calculation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, Guid formId, Guid? parentGroupId)
    {
        ArgumentNullException.ThrowIfNull(media);

        foreach (var node in Collection(_container, "groups"))
        {
            var group = new JsonGroup(node, _calculation);
            group.Print(media);
        }
        return media;
    }

    private static IEnumerable<JsonElement> Collection(JsonElement owner, string name)
    {
        if (!owner.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            return Array.Empty<JsonElement>();
        }

        return value.EnumerateArray();
    }
}
