namespace CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;

/// <summary>
/// Represents a criterion that groups multiple child criteria.
/// </summary>
public sealed class GroupCriterion : BaseCriterion
{
	/// <summary>
	/// Gets the list of child criteria grouped by this criterion.
	/// </summary>
	public required IReadOnlyList<BaseCriterion> Childrens { get; init; }
}
