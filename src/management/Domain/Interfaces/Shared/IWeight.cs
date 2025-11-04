namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

/// <summary>
/// Behavioral contract for a weight that exposes both basis points and percent views.
/// </summary>
public interface IWeight
{

    /// <summary>
    /// Returns the percent representation of the weight.
    /// </summary>
    /// <returns>Percent equivalent of the weight.</returns>
    decimal Percent();

    /// <summary>
    /// Returns the basis points representation of the weight.
    /// </summary>
    /// <returns>Basis points equivalent of the weight.</returns>
    ushort BasisPoints();
}
