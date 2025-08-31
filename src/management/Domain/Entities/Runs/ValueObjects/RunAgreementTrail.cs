using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Tracking of viewing and agreement/disagreement by the evaluated subject as an immutable value object.
/// </summary>
public sealed record RunAgreementTrail : IRunAgreementTrail
{
    private readonly DateTime? _viewedAt;
    private readonly RunAgreementStatus? _status;
    private readonly DateTime? _decidedAt;

    /// <summary>
    /// Creates an agreement trail with optional timestamps and status.
    /// </summary>
    public RunAgreementTrail(DateTime? viewedAt, RunAgreementStatus? status, DateTime? decidedAt)
    {
        _viewedAt = viewedAt;
        _status = status;
        _decidedAt = decidedAt;
    }

    /// <summary>
    /// Returns the first view time when present.
    /// </summary>
    public DateTime? ViewedAt() => _viewedAt;

    /// <summary>
    /// Returns the agreement/disagreement status when present.
    /// </summary>
    public RunAgreementStatus? Status() => _status;

    /// <summary>
    /// Returns the decision time when present.
    /// </summary>
    public DateTime? DecidedAt() => _decidedAt;
}
