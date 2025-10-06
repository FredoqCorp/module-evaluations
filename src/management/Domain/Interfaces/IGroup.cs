using System;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a single group.
/// </summary>
public interface IGroup : IRatingContributionSource
{
    /// <summary>
    /// Validates the internal consistency of the group.
    /// </summary>
    void Validate();
}
