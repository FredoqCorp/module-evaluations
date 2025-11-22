using System.Collections.Immutable;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Collection of rating options.
/// </summary>
public sealed record SimpleRatingOptions : IRatingOptions
{
    private readonly ISeries _series;

    /// <summary>
    /// Initializes the collection from the provided JSON node.
    /// </summary>
    /// <param name="node">JSON element containing the rating options.</param>
    public SimpleRatingOptions(JsonElement node) : this(new SimpleRatingSeries(node.Array<IRatingOption>("ratingOptions", e => new SimpleRatingOption(e)).ToImmutableList()))
    {
    }

    /// <summary>
    /// Initializes the collection with the provided rating options.
    /// </summary>
    /// <param name="options">Enumerable of rating options.</param>
    public SimpleRatingOptions(IImmutableList<IRatingOption> options) : this(new SimpleRatingSeries(options))
    {
    }

    /// <summary>
    /// Initializes the collection with the provided rating options series.
    /// </summary>
    /// <param name="series">Sequence of rating options.</param>
    public SimpleRatingOptions(ISeries series)
    {
        ArgumentNullException.ThrowIfNull(series);
        _series = series;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        await media.WithArrayAsync("ratingOptions", _series.Entries(media));
        return media;
    }
}
