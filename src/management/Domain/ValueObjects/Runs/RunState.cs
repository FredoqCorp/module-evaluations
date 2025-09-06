using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;

/// <summary>
/// Run state: lifecycle, context and results as an immutable value object.
/// </summary>
public sealed record RunState : IRunState
{
    private readonly IRunLifecycle _lifecycle;
    private readonly IRunContext _context;
    private readonly IRunResult _result;

    /// <summary>
    /// Creates a run state snapshot.
    /// </summary>
    public RunState(IRunLifecycle lifecycle, IRunContext context, IRunResult result)
    {
        ArgumentNullException.ThrowIfNull(lifecycle);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(result);
        _lifecycle = lifecycle;
        _context = context;
        _result = result;
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

    
}
