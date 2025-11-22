using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Represents an immutable sequence of groups able to print themselves into media.
/// </summary>
public sealed record SimpleGroupSeries : ISeries
{
    private readonly IReadOnlyCollection<IGroup> _groups;

    /// <summary>
    /// Builds a sequence from the provided groups.
    /// </summary>
    /// <param name="groups">Groups forming the sequence.</param>
    public SimpleGroupSeries(IImmutableList<IGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(groups);
        _groups = [.. groups];
    }

    /// <summary>
    /// Yields delegates that print each group into the supplied media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives each group print.</param>
    /// <returns>Collection of asynchronous print delegates for groups.</returns>
    public IEnumerable<Func<IMedia, Task>> Entries<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        return EntriesOf(media);
    }

    /// <summary>
    /// Provides print delegates for the stored groups using the supplied media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives each group print.</param>
    /// <returns>Collection of asynchronous print delegates for groups.</returns>
    private IEnumerable<Func<IMedia, Task>> EntriesOf<TOutput>(IMedia<TOutput> media)
    {
        foreach (var group in _groups)
        {
            yield return async _ =>
            {
                await group.Print(media);
            };
        }
    }
}
