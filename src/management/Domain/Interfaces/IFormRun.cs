using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Contract for the form run aggregate.
/// </summary>
public interface IFormRun
{
    /// <summary>
    /// Returns the run identifier.
    /// </summary>
    IId Id();

    /// <summary>
    /// Returns the run metadata value object.
    /// </summary>
    IRunMeta Meta();

    /// <summary>
    /// Returns the run state value object.
    /// </summary>
    IRunState State();
}
