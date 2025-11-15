using System.Globalization;
using System.Linq;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms.Models;

/// <summary>
/// JSONB-backed rating options parsed from PostgreSQL payloads.
/// </summary>
internal sealed class PgJsonRatingOptions : IRatingOptions
{
    private readonly string _payload;

    /// <summary>
    /// Stores serialized JSON payload for deferred parsing.
    /// </summary>
    /// <param name="payload">Serialized JSON object keyed by order.</param>
    public PgJsonRatingOptions(string payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        _payload = payload;
    }

    /// <inheritdoc />
    public void Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        var options = Parse(_payload);
        media.WithArray("ratingOptions", Enumerate(options, media));
    }

    /// <summary>
    /// Provides serialization actions for rating options.
    /// </summary>
    private static IEnumerable<Action<IMedia>> Enumerate<TOutput>(IRatingOption[] options, IMedia<TOutput> media)
    {
        foreach (var option in options)
        {
            yield return _ => option.Print(media);
        }
    }

    /// <summary>
    /// Parses the JSON payload into rating option instances.
    /// </summary>
    private static IRatingOption[] Parse(string payload)
    {
        if (payload.Length == 0)
        {
            return [];
        }

        using var document = JsonDocument.Parse(payload);
        if (document.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Rating options payload must be a JSON object");
        }

        var options = new List<(int Order, IRatingOption Option)>();
        foreach (var property in document.RootElement.EnumerateObject())
        {
            var order = int.Parse(property.Name, NumberStyles.Integer, CultureInfo.InvariantCulture);
            options.Add((order, new PgRatingOption(property.Value)));
        }

        options.Sort(static (left, right) => left.Order.CompareTo(right.Order));
        return options.Select(option => option.Option).ToArray();
    }
}
