namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Creation, update and status change stamps as an immutable value object.
/// </summary>
public readonly record struct AuditTrail
{
    /// <summary>
    /// Creates an audit trail with a mandatory creation stamp and update and state change stamps.
    /// </summary>
    public AuditTrail(Stamp created, Stamp updated, Stamp stateChanged)
    {
        Created = created;
        Updated = updated;
        StateChanged = stateChanged;
    }

    /// <summary>
    /// Returns the creation stamp.
    /// </summary>
    public Stamp Created { get; }

    /// <summary>
    /// Returns the last update stamp.
    /// </summary>
    public Stamp Updated { get; }

    /// <summary>
    /// Returns the last status change stamp.
    /// </summary>
    public Stamp StateChanged { get; }
}
