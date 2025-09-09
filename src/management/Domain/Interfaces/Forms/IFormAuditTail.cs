using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for a form audit tail that validates the next action in O(1).
/// </summary>
public interface IFormAuditTail
{
    /// <summary>
    /// Returns the last audit action kind.
    /// </summary>
    FormAuditKind Kind();

    /// <summary>
    /// Returns the last audit stamp.
    /// </summary>
    Stamp Stamp();

    /// <summary>
    /// Accepts the next action and returns the next tail or throws on violation.
    /// </summary>
    IFormAuditTail Accept(FormAuditKind kind, Stamp stamp);
}

