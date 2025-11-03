using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Behavioral contract for a form aggregate that manages lifecycle and metadata.
/// </summary>
public interface IForm
{
    /// <summary>
    /// Prints the form aggregate into the provided media for serialization or persistence.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Media that receives the structured representation.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media);
}
