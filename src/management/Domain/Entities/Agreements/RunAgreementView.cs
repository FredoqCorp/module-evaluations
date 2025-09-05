using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Agreements;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Agreements;

/// <summary>
/// Operator view fact for a published run identified by a shared agreement identifier.
/// </summary>
public sealed class RunAgreementView : IRunAgreementView
{
    private readonly IId _id;
    private readonly IId _runId;
    private readonly DateTime _viewedAt;

    /// <summary>
    /// Creates a view fact with identifiers and view time.
    /// </summary>
    public RunAgreementView(AgreementId id, RunId runId, DateTime viewedAt)
    {
        _id = id;
        _runId = runId;
        _viewedAt = viewedAt;
    }

    /// <summary>
    /// Returns the agreement identifier shared across agreement records.
    /// </summary>
    public IId Id() => _id;

    /// <summary>
    /// Returns the identifier of the run this view refers to.
    /// </summary>
    public IId RunId() => _runId;

    /// <summary>
    /// Returns the first view time.
    /// </summary>
    public DateTime ViewedAt() => _viewedAt;
}
