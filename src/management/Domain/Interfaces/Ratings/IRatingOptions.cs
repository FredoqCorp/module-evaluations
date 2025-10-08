namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

/// <summary>
/// Behavioral contract for a collection of rating options with selection capability.
/// </summary>
public interface IRatingOptions
{

    /// <summary>
    /// Calculates the total contribution produced by the selected option, if any.
    /// </summary>
    /// <returns>The contribution that should participate in downstream scoring.</returns>
    IRatingContribution Contribution();
}
