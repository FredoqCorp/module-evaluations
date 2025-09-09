using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Immutable form audit entry storing kind, stamp and optional change set.
/// </summary>
public sealed record FormAuditEntry : IFormAuditEntry
{
    private readonly EvaluationFormId _form;
    private readonly FormAuditKind _kind;
    private readonly Stamp _stamp;
    private readonly IImmutableList<FormChange> _changes;

    /// <summary>
    /// Creates an entry with a kind, a stamp and an immutable change list.
    /// </summary>
    public FormAuditEntry(EvaluationFormId form, FormAuditKind kind, Stamp stamp, IImmutableList<FormChange> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        _form = form;
        _kind = kind;
        _stamp = stamp;
        _changes = changes;
    }

    /// <summary>
    /// Returns the identifier of the form this entry belongs to.
    /// </summary>
    public EvaluationFormId Form() => _form;

    /// <summary>
    /// Returns the audit kind.
    /// </summary>
    public FormAuditKind Kind() => _kind;

    /// <summary>
    /// Returns the actor and time stamp.
    /// </summary>
    public Stamp Stamp() => _stamp;

    /// <summary>
    /// Returns the immutable collection of changes when applicable.
    /// </summary>
    public IImmutableList<FormChange> Changes() => _changes;
}
