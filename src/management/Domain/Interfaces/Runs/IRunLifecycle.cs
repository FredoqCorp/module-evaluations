using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for run lifecycle stamps.
/// </summary>
public interface IRunLifecycle
{
    /// <summary>
    /// Returns the launch stamp.
    /// </summary>
    Stamp Launched();

    /// <summary>
    /// Returns the first save stamp.
    /// </summary>
    Stamp FirstSaved();

    /// <summary>
    /// Returns the last save stamp.
    /// </summary>
    Stamp LastSaved();

    /// <summary>
    /// Returns the publish stamp.
    /// </summary>
    Stamp Published();
}
