using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run state: lifecycle, context, results and agreement trail as an immutable value object.
/// </summary>
public sealed record RunState : IRunState
{
    private readonly IRunLifecycle _lifecycle;
    private readonly IRunContext _context;
    private readonly IRunResult _result;
    private readonly IRunAgreementTrail _agreement;

    /// <summary>
    /// Creates a run state snapshot.
    /// </summary>
    public RunState(IRunLifecycle lifecycle, IRunContext context, IRunResult result, IRunAgreementTrail agreement)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(agreement);
        _lifecycle = lifecycle;
        _context = context;
        _result = result;
        _agreement = agreement;
    }

    /// <summary>
    /// Returns the run lifecycle object.
    /// </summary>
    public IRunLifecycle Lifecycle() => _lifecycle;

    /// <summary>
    /// Returns the run context object.
    /// </summary>
    public IRunContext Context() => _context;

    /// <summary>
    /// Returns the aggregated result object.
    /// </summary>
    public IRunResult Result() => _result;

    /// <summary>
    /// Returns the agreement trail.
    /// </summary>
    public IRunAgreementTrail Agreement() => _agreement;
}
