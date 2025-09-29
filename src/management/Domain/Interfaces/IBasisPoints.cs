namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for basis points representation that exposes a percent view of the same weight.
/// </summary>
public interface IBasisPoints
{
    /// <summary>
    /// Returns a percent representation computed from the stored basis points.
    /// </summary>
    /// <returns>Percent value equivalent to the stored basis points.</returns>
    IPercent Percent();
}
