using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Identity;

/// <summary>
/// Behavioral contract for user identification and metadata.
/// </summary>
public interface IUserInfo
{
    /// <summary>
    /// Prints the user information into the provided media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output the media produces.</typeparam>
    /// <param name="media">Target media that receives the printed representation.</param>
    void Print<TOutput>(IMedia<TOutput> media);
}
