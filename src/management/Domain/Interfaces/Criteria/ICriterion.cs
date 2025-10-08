namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a single criterion.
/// </summary>
public interface ICriterion : IRatingContributionSource
{
    /// <summary>
    /// Validates the internal consistency of the criterion.
    /// </summary>
    void Validate();

}
