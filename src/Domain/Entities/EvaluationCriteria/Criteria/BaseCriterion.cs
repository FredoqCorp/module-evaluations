namespace CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;

/// <summary>
/// Represents the base class for evaluation criteria.
/// </summary>
public abstract class BaseCriterion
{
    /// <summary>
    /// Gets the unique identifier for the criterion.
    /// </summary>
    public long Id { get; init; }
    /// <summary>
    /// Gets the title of the criterion.
    /// </summary>
    public required string Title { get; init; }
    /// <summary>
    /// Gets the weight of the criterion.
    /// </summary>
    public required decimal Weight { get; init; }
    /// <summary>
    /// Gets the order of the criterion.
    /// </summary>
    public required int Order { get; init; }
}
