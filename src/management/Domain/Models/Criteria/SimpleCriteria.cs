using System.Collections.Immutable;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Collection of criteria.
/// </summary>
public sealed record SimpleCriteria : ICriteria
{
    private readonly ISeries _series;

    /// <summary>
    /// Creates a JSON-backed criteria collection.
    /// </summary>
    /// <param name="container">JSON element containing the criteria array.</param>
    /// <param name="selector">Function to select the criterion based on calculation type and JSON element.</param>
    /// <param name="calculation">Calculation strategy used by the parent form.</param>
    public SimpleCriteria(JsonElement container, Func<CalculationType, JsonElement, ICriterion> selector, CalculationType calculation) : this(new SimpleCriterionSeries(container.Array("criteria", e => selector(calculation, e)).ToImmutableList()))
    {
    }

    /// <summary>
    /// Creates a criteria collection from a list of criteria.
    /// </summary>
    /// <param name="criteria">List of criteria.</param>
    public SimpleCriteria(IImmutableList<ICriterion> criteria) : this(new SimpleCriterionSeries(criteria))
    {
    }

    /// <summary>
    /// Creates a criteria collection from a series.
    /// </summary>
    /// <param name="series">Series of criteria.</param>
    public SimpleCriteria(ISeries series)
    {
        ArgumentNullException.ThrowIfNull(series);
        _series = series;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        await media.WithArrayAsync("criteria", _series.Entries(media));
        return media;
    }
}
