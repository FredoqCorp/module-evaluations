namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Creation, update and status change stamps.
/// </summary>
public sealed class AuditTrail
{
    /// <summary>
    /// Creation stamp.
    /// </summary>
    public required Stamp Created { get; init; }

    /// <summary>
    /// Last update stamp (if any).
    /// </summary>
    public Stamp? Updated { get; init; }

    /// <summary>
    /// Last status change stamp (if any).
    /// </summary>
    public Stamp? StateChanged { get; init; }
}
