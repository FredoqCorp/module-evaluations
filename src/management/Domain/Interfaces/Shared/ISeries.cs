using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

/// <summary>
/// Describes a sequence of items that can expose printable entries for media rendering.
/// </summary>
public interface ISeries
{
    /// <summary>
    /// Provides entries that print each item into the provided media instance.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives each item print.</param>
    /// <returns>Collection of asynchronous print delegates for items.</returns>
    IEnumerable<Func<IMedia, Task>> Entries<TOutput>(IMedia<TOutput> media);
}
