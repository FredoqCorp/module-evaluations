using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;

/// <summary>
/// Lifecycle state, validity period and audit as an immutable value object.
/// </summary>
public sealed record FormLifecycle : IFormLifecycle
{
    private readonly FormStatus _status;
    private readonly IPeriod _validity;
    private readonly IAuditTrail _audit;

    /// <summary>
    /// Creates a lifecycle with a status, optional validity period and audit trail.
    /// </summary>
    public FormLifecycle(FormStatus status, IPeriod validity, IAuditTrail audit)
    {
        ArgumentNullException.ThrowIfNull(audit);
        ArgumentNullException.ThrowIfNull(validity);
        _status = status;
        _validity = validity;
        _audit = audit;
    }

    /// <summary>
    /// Returns the lifecycle status.
    /// </summary>
    public FormStatus Status()
    {
        return _status;
    }

    /// <summary>
    /// Returns the validity period when present.
    /// </summary>
    public IPeriod Validity()
    {
        return _validity;
    }

    /// <summary>
    /// Returns the audit trail.
    /// </summary>
    public IAuditTrail Audit()
    {
        return _audit;
    }
}
