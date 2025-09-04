using CascVel.Modules.Evaluations.Management.Domain.Entities.Agreements.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Agreements;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Agreements;

/// <summary>
/// Operator decision for a published run identified by a shared agreement identifier.
/// </summary>
public sealed class RunAgreementDecision : IRunAgreementDecision
{
    private readonly IId _id;
    private readonly IId _runId;
    private readonly RunAgreementStatus _status;
    private readonly DateTime _decidedAt;
    private readonly string _comment;

    /// <summary>
    /// Creates a decision record with identifiers, status, decision time and optional comment.
    /// </summary>
    public RunAgreementDecision(AgreementId id, RunId runId, RunAgreementStatus status, DateTime decidedAt, string? comment)
    {
        _id = id;
        _runId = runId;
        _status = status;
        _decidedAt = decidedAt;
        _comment = comment ?? string.Empty;
    }

    /// <summary>
    /// Returns the agreement identifier shared across agreement records.
    /// </summary>
    public IId Id() => _id;

    /// <summary>
    /// Returns the identifier of the run this decision refers to.
    /// </summary>
    public IId RunId() => _runId;

    /// <summary>
    /// Returns the decision status.
    /// </summary>
    public RunAgreementStatus Status() => _status;

    /// <summary>
    /// Returns the decision time.
    /// </summary>
    public DateTime DecidedAt() => _decidedAt;

    /// <summary>
    /// Returns the operator comment as a non-null string.
    /// </summary>
    public string Comment() => _comment;
}
