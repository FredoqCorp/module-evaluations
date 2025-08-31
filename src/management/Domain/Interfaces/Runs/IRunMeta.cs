namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for run metadata.
/// </summary>
public interface IRunMeta
{
    /// <summary>
    /// Returns the reference to the form used by the run.
    /// </summary>
    IRunFormRef Form();

    /// <summary>
    /// Returns the identifier of the evaluated subject.
    /// </summary>
    string RunFor();

    /// <summary>
    /// Returns the supervisor comment as a non-null string.
    /// </summary>
    string SupervisorComment();
}

