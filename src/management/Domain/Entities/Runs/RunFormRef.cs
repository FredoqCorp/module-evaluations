namespace CascVel.Module.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Reference to the evaluation form used for this run.
/// </summary>
public sealed class RunFormRef
{
    /// <summary>
    /// Form identifier.
    /// </summary>
    public required long FormId { get; init; }

    /// <summary>
    /// Immutable form code captured at launch time (for stable references in external systems).
    /// </summary>
    public required string FormCode { get; init; }
}
