namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

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
    /// Returns the first save stamp when present.
    /// </summary>
    IStamp? FirstSaved();

    /// <summary>
    /// Returns the last save stamp when present.
    /// </summary>
    IStamp? LastSaved();

    /// <summary>
    /// Returns the publish stamp when present.
    /// </summary>
    IStamp? Published();
}

