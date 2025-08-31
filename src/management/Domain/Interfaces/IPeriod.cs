namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Contract for a period of validity.
/// </summary>
public interface IPeriod
{
    /// <summary>
    /// Returns the inclusive start of the period.
    /// </summary>
    DateTime Start();

    /// <summary>
    /// Returns the inclusive finish of the period when present.
    /// </summary>
    DateTime? Finish();
}
