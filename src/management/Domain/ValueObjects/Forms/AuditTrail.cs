using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Creation, update and status change stamps as an immutable value object.
/// </summary>
public sealed record AuditTrail : IAuditTrail
{
    private readonly Stamp _created;
    private readonly Stamp _updated;
    private readonly Stamp _stateChanged;

    /// <summary>
    /// Creates an audit trail with a mandatory creation stamp and update and state change stamps.
    /// </summary>
    public AuditTrail(Stamp created, Stamp updated, Stamp stateChanged)
    {
        _created = created;
        _updated = updated;
        _stateChanged = stateChanged;
    }

    /// <summary>
    /// Returns the creation stamp.
    /// </summary>
    public Stamp Created()
    {
        return _created;
    }

    /// <summary>
    /// Returns the last update stamp.
    /// </summary>
    public Stamp Updated()
    {
        return _updated;
    }

    /// <summary>
    /// Returns the last status change stamp.
    /// </summary>
    public Stamp StateChanged()
    {
        return _stateChanged;
    }
}
