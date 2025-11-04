using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable entity that encapsulates a printable collection of form summaries.
/// </summary>
internal sealed record FormSummaries : IFormSummaries
{
    private readonly IReadOnlyCollection<IFormSummary> _items;

    /// <summary>
    /// Initializes the collection with the provided summaries.
    /// </summary>
    /// <param name="items">Collection of form summaries.</param>
    public FormSummaries(IReadOnlyCollection<IFormSummary> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = items;
    }

    /// <summary>
    /// Prints the collection to the provided media.
    /// </summary>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <returns>The media instance that received the printed representation.</returns>
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);

        media.WithArray("forms", Items(_items, media));
        return media;
    }

    /// <summary>
    /// Returns the read-only collection of summaries.
    /// </summary>
    /// <returns>Collection of summaries.</returns>
    public IReadOnlyCollection<IFormSummary> Values()
    {
        return _items;
    }

    private static IEnumerable<Action<IMedia>> Items<TOutput>(IEnumerable<IFormSummary> options, IMedia<TOutput> media)
    {
        foreach (var option in options)
        {
            yield return _ => option.Print(media);
        }
    }
}
