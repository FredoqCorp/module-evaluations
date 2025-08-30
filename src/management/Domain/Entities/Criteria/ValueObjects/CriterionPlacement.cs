namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.ValueObjects;

/// <summary>
/// Value object describing how a criterion is placed within a form or section.
/// </summary>
public sealed record CriterionPlacement
{
    /// <summary>
    /// Display order within the form.
    /// </summary>
    public required int Order { get; init; }

    /// <summary>
    /// Weight of the criterion in the form's scoring context.
    /// </summary>
    public required decimal Weight { get; init; }
}
