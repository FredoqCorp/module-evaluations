namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

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

    /// <summary>
    /// Applies the basis points value to the given decimal value.
    /// </summary>
    /// <param name="value">The value to apply the basis points to.</param>
    /// <returns>The result of applying the basis points to the value.</returns>
    decimal Apply(decimal value);
}
