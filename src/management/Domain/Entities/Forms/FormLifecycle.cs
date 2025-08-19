using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Lifecycle state, validity period and audit.
/// </summary>
public sealed class FormLifecycle
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
