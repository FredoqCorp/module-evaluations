using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Represents an immutable sequence of rating options able to print themselves into media.
/// </summary>
public sealed record SimpleRatingSeries : ISeries
{
    private readonly IReadOnlyCollection<IRatingOption> _options;

    /// <summary>
    /// Builds a sequence from the provided rating options.
    /// </summary>
    /// <param name="options">Rating options forming the sequence.</param>
    public SimpleRatingSeries(IImmutableList<IRatingOption> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = [.. options];
    }

    /// <inheritdoc />
    public IEnumerable<Func<IMedia, Task>> Entries<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        return EntriesOf(media);
    }

    /// <summary>
    /// Provides print delegates for the stored rating options using the supplied media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives each rating option print.</param>
    /// <returns>Collection of asynchronous print delegates for rating options.</returns>
    private IEnumerable<Func<IMedia, Task>> EntriesOf<TOutput>(IMedia<TOutput> media)
    {
        foreach (var option in _options)
        {
            yield return async _ =>
            {
                option.Print(media);
                await Task.CompletedTask;
            };
        }
    }
}
