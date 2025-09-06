using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;

/// <summary>
/// Run lifecycle: launch, first and last save, and publish as an immutable value object.
/// </summary>
public sealed record RunLifecycle : IRunLifecycle
{
    private readonly Stamp _launched;
    private readonly Stamp _firstSaved;
    private readonly Stamp _lastSaved;
    private readonly Stamp _published;

    /// <summary>
    /// Creates a run lifecycle value object.
    /// </summary>
    public RunLifecycle(Stamp launched, Stamp firstSaved, Stamp lastSaved, Stamp published)
    {
        _launched = launched;
        _firstSaved = firstSaved;
        _lastSaved = lastSaved;
        _published = published;
    }

    /// <summary>
    /// Returns the launch stamp.
    /// </summary>
    public Stamp Launched() => _launched;

    /// <summary>
    /// Returns the first save stamp.
    /// </summary>
    public Stamp FirstSaved() => _firstSaved;

    /// <summary>
    /// Returns the last save stamp.
    /// </summary>
    public Stamp LastSaved() => _lastSaved;

    /// <summary>
    /// Returns the publish stamp.
    /// </summary>
    public Stamp Published() => _published;
}
