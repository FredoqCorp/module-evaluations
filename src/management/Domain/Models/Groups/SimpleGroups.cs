using System.Collections.Immutable;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Represents a groups collection backed by JSON.
/// </summary>
public sealed record SimpleGroups : IGroups
{
    private readonly ISeries _series;

    /// <summary>
    /// Creates a JSON-backed groups collection.
    /// </summary>
    /// <param name="container">JSON element containing the groups array.</param>
    /// <param name="selector">Function to select the appropriate group type based on calculation strategy.</param>
    /// <param name="calculation">Calculation strategy.</param>
    public SimpleGroups(JsonElement container, Func<CalculationType, JsonElement, IGroup> selector, CalculationType calculation) :
        this(container.Array("groups", e => selector(calculation, e)))
    {
    }

    /// <summary>
    /// Creates a JSON-backed groups collection.
    /// </summary>
    /// <param name="groups">Sequence of groups.</param>
    public SimpleGroups(IEnumerable<IGroup> groups) : this(new SimpleGroupSeries(groups.ToImmutableList()))
    {
    }

    /// <summary>
    /// Creates a JSON-backed groups collection.
    /// </summary>
    /// <param name="series">Sequence of groups.</param>
    public SimpleGroups(ISeries series)
    {
        ArgumentNullException.ThrowIfNull(series);
        _series = series;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        await media.WithArrayAsync("groups", _series.Entries(media));
        return media;
    }
}
