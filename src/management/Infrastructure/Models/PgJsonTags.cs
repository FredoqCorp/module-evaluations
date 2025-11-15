using System.Collections.Immutable;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Models;

/// <summary>
/// JSONB-backed tags collection loaded from PostgreSQL payloads.
/// </summary>
internal sealed class PgJsonTags : ITags
{
    private readonly Lazy<ImmutableList<ITag>> _tags;

    /// <summary>
    /// Initializes the tags collection from serialized JSON payload.
    /// </summary>
    /// <param name="payload">Serialized JSON array of strings.</param>
    public PgJsonTags(string payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        _tags = new Lazy<ImmutableList<ITag>>(() => Read(payload));
    }

    /// <inheritdoc />
    public ITags With(ITag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);
        var value = _tags.Value;
        var text = tag.Text();
        if (value.Any(existing => string.Equals(existing.Text(), text, StringComparison.OrdinalIgnoreCase)))
        {
            return this;
        }

        return new Tags(value.Add(tag));
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
    /// Parses tag payload preserving order and uniqueness.
    /// </summary>
    /// <param name="payload">Serialized JSON array of strings.</param>
    /// <returns>Immutable list of tag value objects.</returns>
    private static ImmutableList<ITag> Read(string payload)
    {
        if (payload.Length == 0)
        {
            return [];
        }

        using var document = JsonDocument.Parse(payload);
        if (document.RootElement.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var unique = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var builder = ImmutableList.CreateBuilder<ITag>();
        foreach (var node in document.RootElement.EnumerateArray())
        {
            var text = node.GetString();
            if (string.IsNullOrWhiteSpace(text))
            {
                continue;
            }

            if (unique.Add(text))
            {
                builder.Add(new Tag(text));
            }
        }

        return builder.ToImmutable();
    }
}
