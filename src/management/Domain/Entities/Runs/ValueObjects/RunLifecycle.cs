using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run lifecycle: launch, first and last save, and publish.
/// </summary>
public sealed record RunLifecycle
{
    /// <summary>
    /// Launch timestamp and actor.
    /// </summary>
    public required Stamp Launched { get; init; }

    /// <summary>
    /// First save (for draft), if any.
    /// </summary>
    public Stamp? FirstSaved { get; init; }

    /// <summary>
    /// Last save, if any.
    /// </summary>
    public Stamp? LastSaved { get; init; }

    /// <summary>
    /// Publish stamp (who/when), if any.
    /// </summary>
    public Stamp? Published { get; init; }
}
