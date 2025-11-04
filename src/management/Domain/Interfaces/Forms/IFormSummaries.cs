using System.Collections.Generic;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Behavioral contract for a collection of form summaries that can be printed into media.
/// </summary>
public interface IFormSummaries
{
    /// <summary>
    /// Prints the collection of form summaries into the provided media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output the media produces.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media);

    /// <summary>
    /// Provides read-only access to the underlying summaries.
    /// </summary>
    /// <returns>Collection of form summaries.</returns>
    IReadOnlyCollection<IFormSummary> Values();
}
