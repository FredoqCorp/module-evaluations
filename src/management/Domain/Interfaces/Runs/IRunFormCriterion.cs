using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

/// <summary>
/// Contract for a run-level decorated form criterion captured at launch time.
/// Implements the form criterion contract while adding a stable run-local key.
/// </summary>
public interface IRunFormCriterion : IFormCriterion
{
}
