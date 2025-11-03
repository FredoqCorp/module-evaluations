using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Represents a criteria collection backed by JSON.
/// </summary>
internal sealed class JsonCriteria : ICriteria
{
    private readonly JsonElement _container;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a JSON-backed criteria collection.
    /// </summary>
    /// <param name="container">JSON element containing the criteria array.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public JsonCriteria(JsonElement container, CalculationType calculation)
    {
        _container = container;
        _calculation = calculation;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        var array = Array.Empty<JsonElement>();
        if (_container.TryGetProperty("criteria", out var value) && value.ValueKind == JsonValueKind.Array)
        {
            array = [.. value.EnumerateArray()];
        }
        var criteria = new List<ICriterion>();
        foreach (var node in array)
        {
            if (_calculation == CalculationType.WeightedAverage)
            {
                var criterion = new JsonNewWeightedCriterion(node, new JsonNewAverageCriterion(node));
                criteria.Add(criterion);
            }
            else
            {
                var criterion = new JsonNewAverageCriterion(node);
                criteria.Add(criterion);
            }
        }
        media.WithArray("criteria", Items(criteria, media));
        return media;
    }

    private static IEnumerable<Action<IMedia>> Items<TOutput>(IEnumerable<ICriterion> options, IMedia<TOutput> media)
    {
        foreach (var option in options)
        {
            yield return _ => option.Print(media);
        }
    }
}
