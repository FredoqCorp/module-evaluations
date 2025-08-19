using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Criterion positioned within a form or a group, with order and optional weight.
/// </summary>
public sealed class FormCriterion
{
    /// <summary>
    /// Unique identifier of the form criterion.
    /// </summary>
    public required long Id { get; init; }

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
