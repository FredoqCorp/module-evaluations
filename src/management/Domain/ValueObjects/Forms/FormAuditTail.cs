using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Immutable tail of the form audit storing the last action kind and its stamp.
/// </summary>
public sealed record FormAuditTail : IFormAuditTail
{
    private readonly FormAuditKind _kind;
    private readonly Stamp _stamp;

    /// <summary>
    /// Creates a new tail with a kind and a stamp.
    /// </summary>
    public FormAuditTail(FormAuditKind kind, Stamp stamp)
    {
        _kind = kind;
        _stamp = stamp;
    }

    /// <summary>
    /// Returns the last audit action kind.
    /// </summary>
    public FormAuditKind Kind() => _kind;

    /// <summary>
    /// Accepts the next action and returns the next tail or throws on violation.
    /// </summary>
    public IFormAuditTail Accept(FormAuditKind kind, Stamp stamp)
    {
        if (stamp.At < _stamp.At) throw new InvalidOperationException("Audit timestamp is out of order");

        return _kind switch
        {
            FormAuditKind.Created => AcceptFromCreated(kind, stamp),
            FormAuditKind.Edited => AcceptFromEdited(kind, stamp),
            FormAuditKind.Published => AcceptFromPublished(kind, stamp),
            FormAuditKind.Archived => throw new InvalidOperationException("Form is already archived"),
            _ => throw new InvalidOperationException("Unknown audit state")
        };
    }

    private static FormAuditTail AcceptFromCreated(FormAuditKind kind, Stamp stamp)
    {
        return kind switch
        {
            FormAuditKind.Edited => new FormAuditTail(FormAuditKind.Edited, stamp),
            FormAuditKind.Published => new FormAuditTail(FormAuditKind.Published, stamp),
            FormAuditKind.Archived => new FormAuditTail(FormAuditKind.Archived, stamp),
            FormAuditKind.Created => throw new InvalidOperationException("Form is already created"),
            _ => throw new InvalidOperationException("Unknown audit transition")
        };
    }

    private static FormAuditTail AcceptFromEdited(FormAuditKind kind, Stamp stamp)
    {
        return kind switch
        {
            FormAuditKind.Edited => new FormAuditTail(FormAuditKind.Edited, stamp),
            FormAuditKind.Published => new FormAuditTail(FormAuditKind.Published, stamp),
            FormAuditKind.Archived => new FormAuditTail(FormAuditKind.Archived, stamp),
            FormAuditKind.Created => throw new InvalidOperationException("Form is already created"),
            _ => throw new InvalidOperationException("Unknown audit transition")
        };
    }

    private static FormAuditTail AcceptFromPublished(FormAuditKind kind, Stamp stamp)
    {
        return kind switch
        {
            FormAuditKind.Archived => new FormAuditTail(FormAuditKind.Archived, stamp),
            FormAuditKind.Edited => throw new InvalidOperationException("Form is already published"),
            FormAuditKind.Published => throw new InvalidOperationException("Form is already published"),
            FormAuditKind.Created => throw new InvalidOperationException("Form is already created"),
            _ => throw new InvalidOperationException("Unknown audit transition")
        };
    }
}
