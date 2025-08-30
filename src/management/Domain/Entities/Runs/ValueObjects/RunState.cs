namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run state: lifecycle, context, results and agreement trail.
/// </summary>
public sealed record RunState
{
    /// <summary>
    /// Run lifecycle (who/when launched, saved, published).
    /// </summary>
    public required RunLifecycle Lifecycle { get; init; }

    /// <summary>
    /// Context parameters for the run.
    /// </summary>
    public required RunContext Context { get; init; }

    /// <summary>
    /// Total and per-criterion assessments.
    /// </summary>
    public required RunResult Result { get; init; }

    /// <summary>
    /// Views and agreement/disagreement after publication.
    /// </summary>
    public RunAgreementTrail? Agreement { get; init; }
}
