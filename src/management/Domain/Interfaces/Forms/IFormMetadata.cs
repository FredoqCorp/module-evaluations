using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Interface for form metadata representation.
/// </summary>
public interface IFormMetadata
{
    /// <summary>
    /// Prints the form metadata to the specified media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output the media produces.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media);
}
