using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a weight that exposes both basis points and percent views.
/// </summary>
public interface IWeight
{

    /// <summary>
    /// Returns the percent representation of the weight.
    /// </summary>
    /// <returns>Percent equivalent of the weight.</returns>
    IPercent Percent();
}
