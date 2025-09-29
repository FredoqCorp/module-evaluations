namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for percentage representation that provides a basis points view with matching granularity.
/// </summary>
public interface IPercent
{
    /// <summary>
    /// Returns a percent representation computed from the stored basis points.
    /// </summary>
    /// <returns>Percent value equivalent to the stored basis points.</returns>
    IBasisPoints Basis();
}
