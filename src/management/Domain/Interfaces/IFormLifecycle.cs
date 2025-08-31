using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

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
    /// Returns the validity period when present.
    /// </summary>
    Period? Validity();

    /// <summary>
    /// Returns the audit trail.
    /// </summary>
    IAuditTrail Audit();
}

