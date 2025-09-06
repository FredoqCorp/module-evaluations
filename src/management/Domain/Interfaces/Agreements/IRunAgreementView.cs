using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Agreements;

/// <summary>
/// Contract for an operator view fact of a published run.
/// </summary>
public interface IRunAgreementView
{
    /// <summary>
    /// Returns the agreement identifier shared across agreement records.
    /// </summary>
    AgreementId Id();

    /// <summary>
    /// Returns the identifier of the run this view refers to.
    /// </summary>
    RunId RunId();

    /// <summary>
    /// Returns the first view time.
    /// </summary>
    DateTime ViewedAt();
}
