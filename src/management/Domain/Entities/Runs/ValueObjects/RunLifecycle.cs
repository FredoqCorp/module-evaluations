using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run lifecycle: launch, first and last save, and publish as an immutable value object.
/// </summary>
public sealed record RunLifecycle : IRunLifecycle
{
    private readonly IStamp _launched;
    private readonly IStamp _firstSaved;
    private readonly IStamp _lastSaved;
    private readonly IStamp _published;

    /// <summary>
    /// Creates a run lifecycle value object.
    /// </summary>
    public RunLifecycle(IStamp launched, IStamp firstSaved, IStamp lastSaved, IStamp published)
    {
        ArgumentNullException.ThrowIfNull(launched);
        ArgumentNullException.ThrowIfNull(firstSaved);
        ArgumentNullException.ThrowIfNull(lastSaved);
        ArgumentNullException.ThrowIfNull(published);
        _launched = launched;
        _firstSaved = firstSaved;
        _lastSaved = lastSaved;
        _published = published;
    }

    /// <summary>
    /// Returns the launch stamp.
    /// </summary>
    public IStamp Launched() => _launched;

    /// <summary>
    /// Returns the first save stamp.
    /// </summary>
    public IStamp FirstSaved() => _firstSaved;

    /// <summary>
    /// Returns the last save stamp.
    /// </summary>
    public IStamp LastSaved() => _lastSaved;

    /// <summary>
    /// Returns the publish stamp.
    /// </summary>
    public IStamp Published() => _published;
}
