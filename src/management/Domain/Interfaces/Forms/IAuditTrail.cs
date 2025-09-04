namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

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
    /// Returns the last update stamp.
    /// </summary>
    IStamp Updated();

    /// <summary>
    /// Returns the last status change stamp.
    /// </summary>
    IStamp StateChanged();
}
