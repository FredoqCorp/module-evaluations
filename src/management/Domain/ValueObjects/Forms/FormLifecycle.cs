using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Lifecycle state, validity period and audit as an immutable value object.
/// </summary>
public sealed record FormLifecycle
{
    /// <summary>
    /// Creates a lifecycle with a status, optional validity period and audit trail.
    /// </summary>
    public FormLifecycle(FormStatus status, Period validity, AuditTrail audit)
    {
        Status = status;
        Validity = validity;
        Audit = audit;
    }

    /// <summary>
    /// Returns the lifecycle status.
    /// </summary>
    public FormStatus Status { get; }

    /// <summary>
    /// Returns the validity period when present.
    /// </summary>
    public Period Validity { get; }

    /// <summary>
    /// Returns the audit trail.
    /// </summary>
    public AuditTrail Audit { get; }
}
