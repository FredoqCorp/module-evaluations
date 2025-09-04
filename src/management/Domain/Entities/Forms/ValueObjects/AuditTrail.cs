using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;

/// <summary>
/// Creation, update and status change stamps as an immutable value object.
/// </summary>
public sealed record AuditTrail : IAuditTrail
{
    private readonly IStamp _created;
    private readonly IStamp _updated;
    private readonly IStamp _stateChanged;

    /// <summary>
    /// Creates an audit trail with a mandatory creation stamp and update and state change stamps.
    /// </summary>
    public AuditTrail(IStamp created, IStamp updated, IStamp stateChanged)
    {
        ArgumentNullException.ThrowIfNull(created);
        ArgumentNullException.ThrowIfNull(updated);
        ArgumentNullException.ThrowIfNull(stateChanged);
        _created = created;
        _updated = updated;
        _stateChanged = stateChanged;
    }

    /// <summary>
    /// Returns the creation stamp.
    /// </summary>
    public IStamp Created()
    {
        return _created;
    }

    /// <summary>
    /// Returns the last update stamp.
    /// </summary>
    public IStamp Updated()
    {
        return _updated;
    }

    /// <summary>
    /// Returns the last status change stamp.
    /// </summary>
    public IStamp StateChanged()
    {
        return _stateChanged;
    }
}
