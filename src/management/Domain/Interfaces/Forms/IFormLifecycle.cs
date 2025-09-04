using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for form lifecycle state, validity and audit.
/// </summary>
public interface IFormLifecycle
{
    /// <summary>
    /// Returns the lifecycle status.
    /// </summary>
    FormStatus Status();

    /// <summary>
    /// Returns the validity period object.
    /// </summary>
    IPeriod Validity();

    /// <summary>
    /// Returns the audit trail.
    /// </summary>
    IAuditTrail Audit();
}
