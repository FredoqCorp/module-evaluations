namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// A group of criteria within a form. Carries a title, order, optional weight and ordered criteria references.
/// </summary>
public sealed record FormGroup
{
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
    public required IReadOnlyList<FormGroup> Groups { get; init; }
}
