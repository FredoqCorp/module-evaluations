using CascVel.Modules.Evaluations.Management.Domain.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Behavioral contract for form calculation strategies.
/// </summary>
public interface IFormCalculation
{
    /// <summary>
    /// Gets the type of calculation strategy.
    /// </summary>
    CalculationType Type();
}
