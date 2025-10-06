namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Interface for Groups
/// </summary>
public interface IGroups : IRatingContributionSource
{
    /// <summary>
    /// Validates the internal consistency of the groups aggregate.
    /// </summary>
    void Validate();
}
