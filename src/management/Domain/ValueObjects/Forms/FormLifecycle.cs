using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Lifecycle state, validity period and audit tail as an immutable value object.
/// </summary>
public sealed record FormLifecycle
{
    /// <summary>
    /// Creates a lifecycle with a status, optional validity period and audit tail.
    /// </summary>
    public FormLifecycle(FormStatus status, Period validity, IFormAuditTail tail)
    {
        Status = status;
        Validity = validity;
        _tail = tail;
    }

    /// <summary>
    /// Returns the lifecycle status.
    /// </summary>
    public FormStatus Status { get; }

    /// <summary>
    /// Returns the validity period when present.
    /// </summary>
    public Period Validity { get; }

    private readonly IFormAuditTail _tail;

    /// <summary>
    /// Returns the audit tail.
    /// </summary>
    public IFormAuditTail Tail() => _tail;
}
