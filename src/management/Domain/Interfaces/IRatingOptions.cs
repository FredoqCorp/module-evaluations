using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a collection of rating options with selection capability.
/// </summary>
public interface IRatingOptions
{
    /// <summary>
    /// Selects a rating option by score from the available options.
    /// </summary>
    /// <param name="score">The score to select.</param>
    /// <returns>A new instance with the selected option.</returns>
    /// <exception cref="Exceptions.ScoreNotFoundException">Thrown when the score is not found in the available options.</exception>
    IRatingOptions WithSelectedScore(RatingScore score);
}
