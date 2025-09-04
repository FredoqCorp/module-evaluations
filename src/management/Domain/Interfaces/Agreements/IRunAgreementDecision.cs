using CascVel.Modules.Evaluations.Management.Domain.Entities.Agreements.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for an operator decision on a published run.
/// </summary>
public interface IRunAgreementDecision
{
    /// <summary>
    /// Returns the agreement identifier shared across agreement records.
    /// </summary>
    IId Id();

    /// <summary>
    /// Returns the identifier of the run this decision refers to.
    /// </summary>
    IId RunId();

    /// <summary>
    /// Returns the decision status.
    /// </summary>
    RunAgreementStatus Status();

    /// <summary>
    /// Returns the decision time.
    /// </summary>
    DateTime DecidedAt();

    /// <summary>
    /// Returns the operator comment as a non-null string.
    /// </summary>
    string Comment();
}
