namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums;

/// <summary>
/// Contract for tracking viewing and agreement/disagreement of a published run.
/// </summary>
public interface IRunAgreementTrail
{
    /// <summary>
    /// Returns the first view time when present.
    /// </summary>
    DateTime? ViewedAt();

    /// <summary>
    /// Returns the agreement/disagreement status when present.
    /// </summary>
    RunAgreementStatus? Status();

    /// <summary>
    /// Returns the decision time when present.
    /// </summary>
    DateTime? DecidedAt();
}

