namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for an operator view fact of a published run.
/// </summary>
public interface IRunAgreementView
{
    /// <summary>
    /// Returns the agreement identifier shared across agreement records.
    /// </summary>
    IId Id();

    /// <summary>
    /// Returns the identifier of the run this view refers to.
    /// </summary>
    IId RunId();

    /// <summary>
    /// Returns the first view time.
    /// </summary>
    DateTime ViewedAt();
}
