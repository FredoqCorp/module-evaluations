using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Represents an immutable sequence of criteria able to print themselves into media.
/// </summary>
public sealed record SimpleCriterionSeries : ISeries
{
    private readonly IReadOnlyCollection<ICriterion> _criteria;

    /// <summary>
    /// Builds a sequence from the provided criteria.
    /// </summary>
    /// <param name="criteria">Criteria forming the sequence.</param>
    public SimpleCriterionSeries(IImmutableList<ICriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        _criteria = [.. criteria];
    }

    /// <inheritdoc />
    public IEnumerable<Func<IMedia, Task>> Entries<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        return Enumerate(media);
    }

    /// <summary>
    /// Provides print delegates for the stored criteria using the supplied media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives each criterion print.</param>
    /// <returns>Collection of asynchronous print delegates for criteria.</returns>
    private IEnumerable<Func<IMedia, Task>> Enumerate<TOutput>(IMedia<TOutput> media)
    {
        foreach (var criterion in _criteria)
        {
            yield return async _ =>
            {
                await criterion.Print(media);
            };
        }
    }
}
