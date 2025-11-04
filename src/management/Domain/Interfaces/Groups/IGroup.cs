using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Behavioral contract for a single group.
/// </summary>
public interface IGroup
{
    /// <summary>
    /// Prints the group into the provided media within its parent context.
    /// </summary>
    /// <typeparam name="TOutput">The type of output produced by the media.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    /// <returns>The media instance that received the printed representation.</returns>
    IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media);
}
