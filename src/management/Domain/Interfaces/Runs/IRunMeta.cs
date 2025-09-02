namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for run metadata.
/// </summary>
public interface IRunMeta
{
    /// <summary>
    /// Returns the snapshot of the form captured at launch time.
    /// </summary>
    IRunFormSnapshot Snapshot();

    /// <summary>
    /// Returns the identifier of the evaluated subject.
    /// </summary>
    string RunFor();

    /// <summary>
    /// Returns the supervisor comment as a non-null string.
    /// </summary>
    string SupervisorComment();
}
