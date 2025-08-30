using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Criterion positioned within a form or a group, with order and optional weight.
/// </summary>
public sealed record FormCriterion
{
    /// <summary>
    /// Domain criterion.
    /// </summary>
    public required Criterion Criterion { get; init; }

    /// <summary>
    /// Display order within its container (form or group).
    /// </summary>
    public required OrderIndex Order { get; init; }

    /// <summary>
    /// Optional weight when WeightedMean is used.
    /// </summary>
    public Weight? Weight { get; init; }
}
