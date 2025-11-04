using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Models.Groups;

/// <summary>
/// Represents a groups collection backed by JSON.
/// </summary>
internal sealed record JsonGroups : IGroups
{
    private readonly JsonElement _container;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a JSON-backed groups collection.
    /// </summary>
    /// <param name="container">JSON element containing the groups array.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonGroups(JsonElement container, CalculationType calculation)
    {
        _container = container;
        _calculation = calculation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        var array = Array.Empty<JsonElement>();
        if (_container.TryGetProperty("groups", out var value) && value.ValueKind == JsonValueKind.Array)
        {
            array = [.. value.EnumerateArray()];
        }
        var groups = new List<IGroup>();
        foreach (var node in array)
        {
            if (_calculation == CalculationType.WeightedAverage)
            {
                var group = new JsonNewWeightedGroup(node);
                groups.Add(group);
            }
            else
            {
                var group = new JsonNewAverageGroup(node);
                groups.Add(group);
            }
        }
        media.WithArray("groups", Items(groups, media));
        return media;
    }

    private static IEnumerable<Action<IMedia>> Items<TOutput>(IEnumerable<IGroup> options, IMedia<TOutput> media)
    {
        foreach (var option in options)
        {
            yield return _ => option.Print(media);
        }
    }
}
