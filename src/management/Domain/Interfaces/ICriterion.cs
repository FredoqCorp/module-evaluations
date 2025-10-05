namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a criterion that calculates its final score.
/// </summary>
public interface ICriterion
{
    /// <summary>
    /// Calculates the total contribution produced by this criterion.
    /// </summary>
    /// <returns>The contribution that participates in downstream scoring.</returns>
    IRatingContribution Contribution();

}
