using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// A group of criteria within a form. Carries a title, order, optional weight and ordered criteria references.
/// </summary>
public sealed class FormGroup
{
    /// <summary>
    /// Technical identifier within the aggregate.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Human-friendly title of the group.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Display order of the group in the form.
    /// </summary>
    public required OrderIndex Order { get; init; }

    /// <summary>
    /// Optional weight of the group when WeightedMean is used.
    /// </summary>
    public Weight? Weight { get; init; }

    /// <summary>
    /// Criteria inside the group, ordered.
    /// </summary>
    public required IReadOnlyList<FormCriterion> Criteria { get; init; }

    /// <summary>
    /// Nested groups inside this group
    /// </summary>
    public IReadOnlyList<FormGroup> Groups { get; init; } = Array.Empty<FormGroup>();
}
