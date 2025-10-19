using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Behavioral contract for a collection of rating options with selection capability.
/// </summary>
public interface IRatingOptions
{
    /// <summary>
    /// Prints the rating options to the provided media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output the media produces.</typeparam>
    /// <param name="media">The media to print to.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    void Print<TOutput>(IMedia<TOutput> media);
}
