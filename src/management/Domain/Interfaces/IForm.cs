using CascVel.Modules.Evaluations.Management.Domain.Common;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a form aggregate that manages lifecycle and metadata.
/// </summary>
public interface IForm
{
    /// <summary>
    /// Validates the internal consistency of the form aggregate.
    /// </summary>
    void Validate();

    /// <summary>
    /// Calculates the final normalized score produced by the form structure.
    /// </summary>
    /// <returns>An option containing the normalized score when participants exist; otherwise None.</returns>
    Option<decimal> Score();
}
