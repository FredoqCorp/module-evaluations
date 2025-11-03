using System.Globalization;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Provides reusable helpers for navigating a form JSON document while enforcing invariants.
/// </summary>
internal static class JsonFormNodes
{
    /// <summary>
    /// Returns a required child section from the provided owner node.
    /// </summary>
    /// <param name="owner">JSON element that owns the child.</param>
    /// <param name="name">Name of the child section.</param>
    /// <returns>Child JSON element.</returns>
    public static JsonElement Section(JsonElement owner, string name)
    {
        if (!owner.TryGetProperty(name, out var value))
        {
            throw new InvalidOperationException($"Missing '{name}' section");
        }

        return value;
    }

    /// <summary>
    /// Returns an enumerable for an optional array child ensuring the correct kind.
    /// </summary>
    /// <param name="owner">JSON element that owns the collection.</param>
    /// <param name="name">Name of the collection property.</param>
    /// <returns>Enumerable over child elements.</returns>
    public static IEnumerable<JsonElement> Collection(JsonElement owner, string name)
    {
        if (!owner.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            return System.Array.Empty<JsonElement>();
        }

        return value.EnumerateArray();
    }

    /// <summary>
    /// Returns a textual property trimmed from leading or trailing whitespace.
    /// </summary>
    /// <param name="node">Node that defines the text property.</param>
    /// <param name="name">Name of the property.</param>
    /// <returns>Trimmed text.</returns>
    public static string Text(JsonElement node, string name)
    {
        if (!node.TryGetProperty(name, out var value))
        {
            throw new InvalidOperationException($"Missing '{name}' property");
        }

        var text = value.GetString() ?? throw new InvalidOperationException($"Property '{name}' must be a string");
        return text.Trim();
    }

    /// <summary>
    /// Returns an optional textual property when present and not null.
    /// </summary>
    /// <param name="node">Node that defines the text property.</param>
    /// <param name="name">Name of the property.</param>
    /// <returns>Trimmed text or null.</returns>
    public static string? OptionalText(JsonElement node, string name)
    {
        if (!node.TryGetProperty(name, out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var text = value.GetString();
        return text?.Trim();
    }

    /// <summary>
    /// Returns description text ensuring an empty result when not provided.
    /// </summary>
    /// <param name="node">Node that may define description.</param>
    /// <returns>Trimmed description.</returns>
    public static string Description(JsonElement node)
    {
        return OptionalText(node, "description") ?? string.Empty;
    }

    /// <summary>
    /// Returns display order ensuring non-negative values.
    /// </summary>
    /// <param name="node">Node that contains the order property.</param>
    /// <returns>Order index.</returns>
    public static int Order(JsonElement node)
    {
        if (!node.TryGetProperty("order", out var value))
        {
            throw new InvalidOperationException("Missing 'order' property");
        }

        var order = value.GetInt32();
        ArgumentOutOfRangeException.ThrowIfNegative(order);
        return order;
    }

    /// <summary>
    /// Returns a weight expressed in basis points when weighted calculation is required.
    /// </summary>
    /// <param name="node">Node that may define the weight.</param>
    /// <param name="calculation">Calculation strategy of the parent context.</param>
    /// <param name="label">Label used for error feedback.</param>
    /// <returns>Weight in basis points or null.</returns>
    public static int? Weight(JsonElement node, CalculationType calculation, string label)
    {
        if (calculation != CalculationType.WeightedAverage)
        {
            if (node.TryGetProperty("weight", out _))
            {
                throw new InvalidOperationException($"Average {label} forbids 'weight'");
            }

            return null;
        }

        if (!node.TryGetProperty("weight", out var value))
        {
            throw new InvalidOperationException($"Weighted {label} requires 'weight'");
        }

        if (value.ValueKind != JsonValueKind.Number)
        {
            throw new InvalidOperationException($"Weighted {label} requires numeric 'weight'");
        }

        var weight = value.GetDecimal();
        if (weight < 0m)
        {
            throw new InvalidOperationException($"Weighted {label} requires non-negative weight");
        }

        if (weight > 100m)
        {
            throw new InvalidOperationException($"Weighted {label} requires weight up to 100");
        }

        var scaled = decimal.Multiply(weight, 100m);
        if (scaled % 1m != 0m)
        {
            throw new InvalidOperationException($"Weighted {label} requires weight with two decimals precision");
        }

        var basis = (int)scaled;
        ArgumentOutOfRangeException.ThrowIfNegative(basis);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(basis, 10000);
        return basis;
    }

    /// <summary>
    /// Returns serialized rating options ensuring unique ordering.
    /// </summary>
    /// <param name="node">Node that defines rating options.</param>
    /// <param name="options">Serializer options used for deterministic output.</param>
    /// <returns>Serialized rating options.</returns>
    public static string Ratings(JsonElement node, JsonSerializerOptions options)
    {
        if (!node.TryGetProperty("ratingOptions", out var value) || value.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Criterion requires 'ratingOptions' array");
        }

        var ratings = new Dictionary<string, RatingOption>();
        JsonElement element;
        foreach (var option in value.EnumerateArray())
        {
            var order = ratings.Count;
            if (option.TryGetProperty("order", out element))
            {
                order = element.GetInt32();
            }

            if (!option.TryGetProperty("label", out element))
            {
                throw new InvalidOperationException("Rating option requires 'label'");
            }

            var label = element.GetString() ?? string.Empty;

            if (!option.TryGetProperty("score", out element))
            {
                throw new InvalidOperationException("Rating option requires 'score'");
            }

            var score = element.GetDecimal();

            var annotation = string.Empty;
            if (option.TryGetProperty("annotation", out element))
            {
                annotation = element.GetString() ?? string.Empty;
            }

            var key = order.ToString(CultureInfo.InvariantCulture);
            if (ratings.ContainsKey(key))
            {
                throw new InvalidOperationException("Duplicate rating option order detected");
            }

            ratings[key] = new RatingOption(score, label, annotation);
        }

        if (ratings.Count == 0)
        {
            throw new InvalidOperationException("Criterion requires at least one rating option");
        }

        return JsonSerializer.Serialize(ratings, options);
    }

    /// <summary>
    /// Returns calculation type token parsed from the root node.
    /// </summary>
    /// <param name="root">Root node that defines the calculation field.</param>
    /// <returns>Calculation strategy.</returns>
    public static CalculationType Calculation(JsonElement root)
    {
        var token = Text(root, "calculation");
        if (string.Equals(token, "average", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.Average;
        }

        if (string.Equals(token, "weighted", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.WeightedAverage;
        }

        throw new InvalidOperationException("Unsupported calculation token");
    }

    /// <summary>
    /// Returns calculation token for persistence.
    /// </summary>
    /// <param name="calculation">Calculation strategy.</param>
    /// <returns>Token persisted with the form.</returns>
    public static string CalculationToken(CalculationType calculation)
    {
        return calculation switch
        {
            CalculationType.Average => "average",
            CalculationType.WeightedAverage => "weighted",
            _ => throw new InvalidOperationException("Unsupported calculation type")
        };
    }

    /// <summary>
    /// Returns tag values trimmed from the metadata section.
    /// </summary>
    /// <param name="node">Metadata node.</param>
    /// <returns>Tag texts.</returns>
    public static IEnumerable<string> Tags(JsonElement node)
    {
        if (!node.TryGetProperty("tags", out var value) || value.ValueKind != JsonValueKind.Array)
        {
            return System.Array.Empty<string>();
        }

        var result = new List<string>();
        foreach (var element in value.EnumerateArray())
        {
            var text = element.GetString();
            if (!string.IsNullOrWhiteSpace(text))
            {
                result.Add(text.Trim());
            }
        }

        return result;
    }

    /// <summary>
    /// Returns the identifier for the form creating it when missing.
    /// </summary>
    /// <param name="node">Root node that may define the identifier.</param>
    /// <returns>Form identity value object.</returns>
    public static FormId FormIdentity(JsonElement node)
    {
        if (node.TryGetProperty("id", out var value) && value.ValueKind == JsonValueKind.String)
        {
            return new FormId(value.GetGuid());
        }

        return new FormId();
    }

    /// <summary>
    /// Returns identifier for groups or criteria creating it when missing.
    /// </summary>
    /// <param name="node">Node that may define the identifier.</param>
    /// <returns>Unique identifier.</returns>
    public static Guid Identifier(JsonElement node)
    {
        if (node.TryGetProperty("id", out var value) && value.ValueKind == JsonValueKind.String)
        {
            return value.GetGuid();
        }

        return Guid.CreateVersion7();
    }

    /// <summary>
    /// Represents a serializable rating option entry used for deterministic output.
    /// </summary>
    private sealed record RatingOption(decimal Score, string Label, string Annotation);
}
