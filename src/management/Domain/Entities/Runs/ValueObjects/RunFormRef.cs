using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Reference to the evaluation form used for this run.
/// </summary>
public sealed record RunFormRef
{
    /// <summary>
    /// Form identifier.
    /// </summary>
    public required Uuid FormId { get; init; }

    /// <summary>
    /// Immutable form code captured at launch time (for stable references in external systems).
    /// </summary>
    public required string FormCode { get; init; }
}
