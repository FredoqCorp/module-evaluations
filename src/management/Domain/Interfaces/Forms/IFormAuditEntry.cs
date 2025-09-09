using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for a single immutable form audit entry.
/// </summary>
public interface IFormAuditEntry
{
    /// <summary>
    /// Returns the identifier of the form this entry belongs to.
    /// </summary>
    EvaluationFormId Form();

    /// <summary>
    /// Returns the audit kind.
    /// </summary>
    FormAuditKind Kind();

    /// <summary>
    /// Returns the actor and time stamp.
    /// </summary>
    Stamp Stamp();

    /// <summary>
    /// Returns the immutable collection of changes when applicable.
    /// </summary>
    IImmutableList<FormChange> Changes();
}
