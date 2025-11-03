using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed collection of rating options that validates ordering and delegates printing.
/// </summary>
public sealed class JsonRatingOptions : IRatingOptions
{
    private readonly JsonElement _node;

    /// <summary>
    /// Initializes the collection from the provided json node
    /// </summary>
    /// <param name="node">JSON element containing the rating options.</param>
    public JsonRatingOptions(JsonElement node)
    {
        _node = node;
    }

    /// <inheritdoc />
    public void Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        if (!_node.TryGetProperty("ratingOptions", out var optionsNode) || optionsNode.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Criterion requires 'ratingOptions' array");
        }
        var options = ToArray(optionsNode.EnumerateArray());
        media.WithArray("ratingOptions", Items(media, options));
    }

    /// <summary>
    /// Produces media writers for every rating option.
    /// </summary>
    /// <returns>Enumerable of actions that write option objects.</returns>
    private static IEnumerable<Action<IMedia>> Items<TOutput>(IMedia<TOutput> media, JsonRatingOption[] options)
    {
        foreach (var option in options)
        {
            yield return _ => option.Print(media);
        }
    }

    private static JsonRatingOption[] ToArray(JsonElement.ArrayEnumerator enumerator)
    {
        var options = new List<JsonRatingOption>();
        var scores = new HashSet<int>();

        foreach (var element in enumerator)
        {
            var option = new JsonRatingOption(element);
            if (!scores.Add(option.Score))
            {
                throw new InvalidOperationException("Duplicate rating option score detected");
            }

            options.Add(option);
        }

        if (options.Count == 0)
        {
            throw new InvalidOperationException("Criterion requires at least one rating option");
        }

        options.Sort(static (left, right) => left.Score.CompareTo(right.Score));
        return [.. options];
    }
}
