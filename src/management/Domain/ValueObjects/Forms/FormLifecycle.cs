using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Lifecycle state, validity period and audit tail as an immutable value object.
/// </summary>
public sealed record FormLifecycle
{
    /// <summary>
    /// Creates a lifecycle with validity period and audit tail.
    /// </summary>
    public FormLifecycle(Period validity, IFormAuditTail tail)
    {
        Validity = validity;
        Tail = tail;
    }

    /// <summary>
    /// Returns the validity period when present.
    /// </summary>
    public Period Validity { get; }

    /// <summary>
    /// Returns the audit tail.
    /// </summary>
    public IFormAuditTail Tail { get; }
}
