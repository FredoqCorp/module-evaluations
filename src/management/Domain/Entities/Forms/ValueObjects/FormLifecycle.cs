using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Lifecycle state, validity period and audit.
/// </summary>
public sealed record FormLifecycle
{
    /// <summary>
    /// Lifecycle status.
    /// </summary>
    public FormStatus Status { get; init; } = FormStatus.Draft;

    /// <summary>
    /// Validity period; null means no constraints. Start is required within Period; End optional.
    /// </summary>
    public Period? Validity { get; init; }

    /// <summary>
    /// Creation and updates audit trail.
    /// </summary>
    public required AuditTrail Audit { get; init; }
}
