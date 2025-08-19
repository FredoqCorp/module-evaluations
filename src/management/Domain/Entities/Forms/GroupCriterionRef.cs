using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Reference to a criterion within a group, with order and optional weight.
/// </summary>
public sealed class GroupCriterionRef
{
    /// <summary>
    /// Referenced criterion.
    /// </summary>
    public required Criterion Criterion { get; init; }

    /// <summary>
    /// Display order of the criterion within the group.
    /// </summary>
    public required OrderIndex Order { get; init; }

    /// <summary>
    /// Optional weight within the group when WeightedMean is used.
    /// </summary>
    public Weight? Weight { get; init; }
}
