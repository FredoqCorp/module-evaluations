namespace CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Text content of a criterion: title and detailed description.
/// </summary>
public sealed record CriterionText
{
    /// <summary>
    /// Human-readable title.
    /// </summary>
    public required CriterionTitle Title { get; init; }

    /// <summary>
    /// Detailed description of the criterion intent.
    /// </summary>
    public required CriterionDescription Description { get; init; }
}
