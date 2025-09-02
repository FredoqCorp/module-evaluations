namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for a run-level decorated form criterion captured at launch time.
/// Implements the form criterion contract while adding a stable run-local key.
/// </summary>
public interface IRunFormCriterion : IFormCriterion
{
    /// <summary>
    /// Returns the run-local unique key of this criterion within the snapshot.
    /// </summary>
    Guid Key();
}

