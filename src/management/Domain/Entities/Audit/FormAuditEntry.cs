using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Audit;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Audit;

/// <summary>
/// Immutable form audit entry storing kind, stamp and optional change set.
/// </summary>
public sealed class FormAuditEntry
{
    /// <summary>
    /// Creates an entry with a kind, a stamp and an immutable change list.
    /// </summary>
    public FormAuditEntry(EvaluationFormId form, FormAuditKind kind, Stamp stamp, IImmutableList<FormChange> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        Form = form;
        Kind = kind;
        Stamp = stamp;
        Changes = changes;
    }

    /// <summary>
    /// Gets the identifier of the evaluation form associated with this audit entry.
    /// </summary>
    public EvaluationFormId Form { get; }

    /// <summary>
    /// Gets the kind of audit event for the evaluation form.
    /// </summary>
    public FormAuditKind Kind { get; }

    /// <summary>
    /// Gets the timestamp of the audit event.
    /// </summary>
    public Stamp Stamp { get; }

    /// <summary>
    /// Gets the list of changes made to the evaluation form.
    /// </summary>
    public IImmutableList<FormChange> Changes { get; }
}
