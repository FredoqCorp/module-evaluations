namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
/// <summary>
/// Contract for a reference to the evaluation form used for the run.
/// </summary>
public interface IRunFormRef
{
    /// <summary>
    /// Returns the form identifier.
    /// </summary>
    IId FormId();

    /// <summary>
    /// Returns the immutable form code captured at launch time.
    /// </summary>
    string FormCode();
}

