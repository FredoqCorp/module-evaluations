namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for a non-negative percentage weight expressed in basis points.
/// </summary>
public interface IWeight
{
    /// <summary>
    /// Returns the weight value as a percentage.
    /// </summary>
    decimal Percent();

    /// <summary>
    /// Returns the weight value in basis points.
    /// </summary>
    ushort Bps();
}

