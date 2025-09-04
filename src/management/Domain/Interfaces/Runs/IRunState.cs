namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for run state snapshot.
/// </summary>
public interface IRunState
{
    /// <summary>
    /// Returns the run lifecycle object.
    /// </summary>
    IRunLifecycle Lifecycle();

    /// <summary>
    /// Returns the run context object.
    /// </summary>
    IRunContext Context();

    /// <summary>
    /// Returns the aggregated result object.
    /// </summary>
    IRunResult Result();
}
