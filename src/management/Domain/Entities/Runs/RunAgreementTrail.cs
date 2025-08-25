using CascVel.Modules.Evaluations.Management.Domain.Enums.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Tracking of viewing and agreement/disagreement by the evaluated subject.
/// </summary>
public sealed class RunAgreementTrail
{
    /// <summary>
    /// Timestamp of the first view of the published evaluation (UTC recommended).
    /// </summary>
    public DateTime? ViewedAt { get; init; }

    /// <summary>
    /// Agreement/disagreement status, if expressed.
    /// </summary>
    public RunAgreementStatus? Status { get; init; }

    /// <summary>
    /// Timestamp when the decision was made.
    /// </summary>
    public DateTime? DecidedAt { get; init; }
}
