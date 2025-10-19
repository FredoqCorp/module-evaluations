using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Behavioral contract for a single rating option within a scale.
/// </summary>
public interface IRatingOption
{
    /// <summary>
    /// Prints the rating option to the provided media.
    /// </summary>
    /// <typeparam name="TOutput">The type of output the media produces.</typeparam>
    /// <param name="media">The media to print to.</param>
    void Print<TOutput>(IMedia<TOutput> media);
}
