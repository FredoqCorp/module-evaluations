namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

public interface IBasisPoints
{
    /// <summary>
    /// Returns a percent representation computed from the stored basis points.
    /// </summary>
    /// <returns>Percent value equivalent to the stored basis points.</returns>
    IPercent Percent();
}
