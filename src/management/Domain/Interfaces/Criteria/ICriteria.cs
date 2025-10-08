using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Behavioral contract for a criteria collection.
/// </summary>
public interface ICriteria : IRatingContributionSource
{
    /// <summary>
    /// Validates the internal consistency of the criteria collection.
    /// </summary>
    void Validate();
}
