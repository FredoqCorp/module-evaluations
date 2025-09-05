using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for run lifecycle stamps.
/// </summary>
public interface IRunLifecycle
{
    /// <summary>
    /// Returns the launch stamp.
    /// </summary>
    IStamp Launched();

    /// <summary>
    /// Returns the first save stamp.
    /// </summary>
    IStamp FirstSaved();

    /// <summary>
    /// Returns the last save stamp.
    /// </summary>
    IStamp LastSaved();

    /// <summary>
    /// Returns the publish stamp.
    /// </summary>
    IStamp Published();
}
