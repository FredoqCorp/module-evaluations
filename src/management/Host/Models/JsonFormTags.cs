using System.Collections.Immutable;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Host.Models;

/// <summary>
/// JSON-backed tags collection that implements the immutable tags contract.
/// </summary>
public sealed class JsonFormTags : ITags
{
    private readonly Lazy<ImmutableList<ITag>> _tags;

    /// <summary>
    /// Initializes the tags collection from the provided metadata node.
    /// </summary>
    /// <param name="metadata">Metadata JSON element that may contain the tags array.</param>
    public JsonFormTags(JsonElement metadata)
    {
        _tags = new Lazy<ImmutableList<ITag>>(() => Read(metadata));
    }

    /// <inheritdoc />
    public ITags With(ITag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);
        var text = tag.Text();
        var snapshot = _tags.Value;
        if (snapshot.Any(existing => string.Equals(existing.Text(), text, StringComparison.OrdinalIgnoreCase)))
        {
            return this;
        }

        return new Tags(snapshot.Add(tag));
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media, string key)
    {
        ArgumentNullException.ThrowIfNull(media);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        media.With(key, _tags.Value.Select(tag => tag.Text()));
        return media;
    }

    /// <summary>
    /// Reads tag texts from the metadata node ensuring uniqueness.
    /// </summary>
    /// <param name="metadata">Metadata JSON element that may contain tags.</param>
    /// <returns>Immutable list of tag texts.</returns>
    private static ImmutableList<ITag> Read(JsonElement metadata)
    {
        if (!metadata.TryGetProperty("tags", out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var unique = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var builder = ImmutableList.CreateBuilder<ITag>();
        foreach (var item in array.EnumerateArray())
        {
            var tag = new ValidTag(new TrimmedTag(new JsonTag(item)));
            if (unique.Add(tag.Text()))
            {
                builder.Add(tag);
            }
        }

        return builder.ToImmutable();
    }
}
