using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Behavioral contract for a single rating option within a scale.
/// </summary>
public interface IRatingOption
{
    /// <summary>
    /// Determines if this rating option matches the given score.
    /// </summary>
    /// <param name="score">The score to compare against.</param>
    /// <returns>True if this option has the given score.</returns>
    bool Matches(RatingScore score);

    /// <summary>
    /// Calculates the contribution of this option to the total form score.
    /// </summary>
    /// <returns>The contribution produced by this option.</returns>
    IRatingContribution Contribution();
}
