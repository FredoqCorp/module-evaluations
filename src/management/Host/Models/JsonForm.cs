using System.Globalization;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// Form aggregate backed by raw JSON payload that can be printed for persistence without exposing getters.
/// </summary>
public sealed class JsonForm : IForm
{
    private static readonly JsonSerializerOptions RatingSerialization = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly JsonDocument _document;
    private readonly FormId _id;
    private readonly FormName _name;
    private readonly FormDescription _description;
    private readonly FormCode _code;
    private readonly string[] _tagTexts;
    private readonly CalculationType _calculation;

    /// <summary>
    /// Creates a printable form aggregate from a JSON document.
    /// </summary>
    /// <param name="document">JSON document that describes the form structure.</param>
    public JsonForm(JsonDocument document)
    {
        ArgumentNullException.ThrowIfNull(document);

        _document = JsonDocument.Parse(document.RootElement.GetRawText());
        var root = _document.RootElement;
        _calculation = Calculation(root);

        var metadataNode = Node(root, "metadata");
        _name = new FormName(Text(metadataNode, "name"));
        _description = new FormDescription(OptionalText(metadataNode, "description") ?? string.Empty);
        _code = new FormCode(Text(metadataNode, "code"));
        var tagValues = TagValues(metadataNode).ToArray();
        _ = new Tags(tagValues.Select(text => new Tag(text)));
        _tagTexts = tagValues;

        _id = Form(root);

        Node(root, "root");
    }

    /// <summary>
    /// Returns the identifier assigned to the JSON-backed form aggregate.
    /// </summary>
    /// <returns>Unique form identifier.</returns>
    public FormId Identity()
    {
        return _id;
    }

    /// <inheritdoc />
    public void Validate()
    {
        var root = Node(_document.RootElement, "root");
        Groups(root);
        Criteria(root);
    }

    /// <inheritdoc />
    public void Print(IMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        var root = Node(_document.RootElement, "root");

        media.With("formId", _id.Value);
        media.With("name", _name.Value);
        media.With("description", _description.Value);
        media.With("code", _code.Token);
        media.With("calculation", CalculationToken(_calculation));
        media.With("tags", _tagTexts);

        var formGuid = _id.Value;
        Criteria(media, root, formGuid, null);
        Groups(media, root, formGuid, null);
    }

    private void Groups(JsonElement container)
    {
        foreach (var group in Array(container, "groups"))
        {
            Group(group);
        }
    }

    private void Groups(IMedia media, JsonElement container, Guid form, Guid? parent)
    {
        foreach (var group in Array(container, "groups"))
        {
            var identifier = Identifier(group);
            Group(media, group, form, parent, identifier);
            Criteria(media, group, form, identifier);
            Groups(media, group, form, identifier);
        }
    }

    private void Criteria(JsonElement container)
    {
        foreach (var criterion in Array(container, "criteria"))
        {
            Criterion(criterion);
        }
    }

    private void Criteria(IMedia media, JsonElement container, Guid form, Guid? parent)
    {
        foreach (var criterion in Array(container, "criteria"))
        {
            Criterion(media, criterion, form, parent);
        }
    }

    private void Group(JsonElement node)
    {
        Identifier(node);
        Text(node, "title");
        Order(node);
        Weight(node, _calculation, "group");
        Description(node);
        foreach (var criterion in Array(node, "criteria"))
        {
            Criterion(criterion);
        }

        foreach (var child in Array(node, "groups"))
        {
            Group(child);
        }
    }

    private void Group(IMedia media, JsonElement node, Guid form, Guid? parent, Guid identifier)
    {
        media.WithObject("group", inner =>
        {
            inner.With("id", identifier);
            inner.With("formId", form);
            if (parent.HasValue)
            {
                inner.With("parentId", parent.Value);
            }

            inner.With("title", Text(node, "title"));
            inner.With("description", Description(node));
            inner.With("groupType", CalculationToken(_calculation));
            inner.With("orderIndex", Order(node));

            var weight = Weight(node, _calculation, "group");
            if (weight.HasValue)
            {
                inner.With("weightBasisPoints", weight.Value);
            }
        });

    }

    private void Criterion(JsonElement node)
    {
        Identifier(node);
        Text(node, "title");
        Text(node, "text");
        Order(node);
        Weight(node, _calculation, "criterion");
        Ratings(node);
    }

    private void Criterion(IMedia media, JsonElement node, Guid form, Guid? parent)
    {
        var identifier = Identifier(node);
        var weight = Weight(node, _calculation, "criterion");
        var ratingOptions = Ratings(node);

        media.WithObject("criterion", inner =>
        {
            inner.With("id", identifier);
            if (parent.HasValue)
            {
                inner.With("groupId", parent.Value);
            }
            else
            {
                inner.With("formId", form);
            }

            inner.With("title", Text(node, "title"));
            inner.With("text", Text(node, "text"));
            inner.With("criterionType", CalculationToken(_calculation));
            inner.With("orderIndex", Order(node));
            inner.With("ratingOptions", ratingOptions);

            if (weight.HasValue)
            {
                inner.With("weightBasisPoints", weight.Value);
            }
        });
    }

    private static FormId Form(JsonElement node)
    {
        if (node.TryGetProperty("id", out var value) && value.ValueKind == JsonValueKind.String)
        {
            return new FormId(value.GetGuid());
        }

        return new FormId();
    }

    private static Guid Identifier(JsonElement node)
    {
        if (node.TryGetProperty("id", out var value) && value.ValueKind == JsonValueKind.String)
        {
            return value.GetGuid();
        }

        return Guid.CreateVersion7();
    }

    private static string Text(JsonElement node, string name)
    {
        if (!node.TryGetProperty(name, out var value))
        {
            throw new InvalidOperationException($"Missing '{name}' property");
        }

        var text = value.GetString() ?? throw new InvalidOperationException($"Property '{name}' must be a string");
        return text.Trim();
    }

    private static string? OptionalText(JsonElement node, string name)
    {
        if (!node.TryGetProperty(name, out var value) || value.ValueKind == JsonValueKind.Null)
        {
            return null;
        }

        var text = value.GetString();
        return text?.Trim();
    }

    private static string Description(JsonElement node)
    {
        return OptionalText(node, "description") ?? string.Empty;
    }

    private static int Order(JsonElement node)
    {
        if (!node.TryGetProperty("order", out var value))
        {
            throw new InvalidOperationException("Missing 'order' property");
        }

        var order = value.GetInt32();
        ArgumentOutOfRangeException.ThrowIfNegative(order);
        return order;
    }

    private static int? Weight(JsonElement node, CalculationType calculation, string label)
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

    private static string Ratings(JsonElement node)
    {
        if (!node.TryGetProperty("ratingOptions", out var options) || options.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Criterion requires 'ratingOptions' array");
        }

        var builder = new Dictionary<string, RatingOption>();
        foreach (var option in options.EnumerateArray())
        {
            var order = option.TryGetProperty("order", out var orderNode)
                ? orderNode.GetInt32()
                : builder.Count;
            var label = option.TryGetProperty("label", out var labelNode)
                ? labelNode.GetString() ?? string.Empty
                : throw new InvalidOperationException("Rating option requires 'label'");
            var score = option.TryGetProperty("score", out var scoreNode)
                ? scoreNode.GetDecimal()
                : throw new InvalidOperationException("Rating option requires 'score'");
            var annotation = option.TryGetProperty("annotation", out var annotationNode)
                ? annotationNode.GetString() ?? string.Empty
                : string.Empty;

            var key = order.ToString(CultureInfo.InvariantCulture);
            if (builder.ContainsKey(key))
            {
                throw new InvalidOperationException("Duplicate rating option order detected");
            }

            builder[key] = new RatingOption(score, label, annotation);
        }

        if (builder.Count == 0)
        {
            throw new InvalidOperationException("Criterion requires at least one rating option");
        }

        return JsonSerializer.Serialize(builder, RatingSerialization);
    }

    private static CalculationType Calculation(JsonElement root)
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

    private static string CalculationToken(CalculationType calculation)
    {
        return calculation switch
        {
            CalculationType.Average => "average",
            CalculationType.WeightedAverage => "weighted",
            _ => throw new InvalidOperationException("Unsupported calculation type")
        };
    }

    private static IEnumerable<string> TagValues(JsonElement node)
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

    private static JsonElement Node(JsonElement owner, string name)
    {
        if (!owner.TryGetProperty(name, out var value))
        {
            throw new InvalidOperationException($"Missing '{name}' section");
        }

        return value;
    }

    private static IEnumerable<JsonElement> Array(JsonElement owner, string name)
    {
        if (!owner.TryGetProperty(name, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            return System.Array.Empty<JsonElement>();
        }

        return value.EnumerateArray();
    }

    private sealed record RatingOption(
        [property: JsonPropertyName("score")] decimal Score,
        [property: JsonPropertyName("label")] string Label,
        [property: JsonPropertyName("annotation")] string Annotation);
}
