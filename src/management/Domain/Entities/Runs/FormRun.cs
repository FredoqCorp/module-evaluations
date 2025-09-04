using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Aggregate representing a form evaluation run.
/// </summary>
public sealed class FormRun : IFormRun
{
    private readonly IId _id;
    private readonly IRunMeta _meta;
    private readonly IRunState _state;

    /// <summary>
    /// Creates a form run aggregate with identifier, metadata and state.
    /// </summary>
    public FormRun(Identifiers.RunId id, IRunMeta meta, IRunState state)
    {
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(state);
        _id = id;
        _meta = meta;
        _state = state;
    }

    /// <summary>
    /// Returns the run identifier of this form run aggregate.
    /// </summary>
    public IId Id() => _id;

    /// <summary>
    /// Returns the run metadata of this form run aggregate.
    /// </summary>
    public IRunMeta Meta() => _meta;

    /// <summary>
    /// Returns the run state of this form run aggregate.
    /// </summary>
    public IRunState State() => _state;
}
