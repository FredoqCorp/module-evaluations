using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for validity periods that evaluate availability and create constrained variants.
/// </summary>
public interface IValidityPeriod
{
    /// <summary>
    /// Returns a new validity period that ends at the provided moment.
    /// </summary>
    /// <param name="end">Moment that closes the validity window.</param>
    /// <returns>Validity period with updated boundary.</returns>
    IValidityPeriod Until(ValidityEnd end);

    /// <summary>
    /// Determines whether the provided moment falls inside the validity window.
    /// </summary>
    /// <param name="moment">Moment to evaluate.</param>
    /// <returns>True when the moment is inside the window.</returns>
    bool Active(DateTime moment);
}
