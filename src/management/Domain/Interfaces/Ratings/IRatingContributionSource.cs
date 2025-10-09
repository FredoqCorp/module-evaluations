namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Behavioral contract for an element that contributes to the rating score.
/// </summary>
public interface IRatingContributionSource
{
    /// <summary>
    /// Calculates the total contribution produced by this criterion.
    /// </summary>
    /// <returns>The contribution that participates in downstream scoring.</returns>
    IRatingContribution Contribution();

}
