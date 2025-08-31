namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Contract for a creation and update audit trail.
/// </summary>
public interface IAuditTrail
{
    /// <summary>
    /// Returns the creation stamp.
    /// </summary>
    IStamp Created();

    /// <summary>
    /// Returns the last update stamp when present.
    /// </summary>
    IStamp? Updated();

    /// <summary>
    /// Returns the last status change stamp when present.
    /// </summary>
    IStamp? StateChanged();
}

