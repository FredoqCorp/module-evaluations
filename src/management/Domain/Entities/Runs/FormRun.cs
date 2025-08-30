using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Aggregate representing a form evaluation run.
/// </summary>
public sealed class FormRun
{
    /// <summary>
    /// Run identifier.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Run metadata (form, subject, comment).
    /// </summary>
    public required RunMeta Meta { get; init; }

    /// <summary>
    /// Run state: lifecycle, context, results and agreement.
    /// </summary>
    public required RunState State { get; init; }
}
