namespace CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;

/// <summary>
/// Represents the default evaluation criterion with a set of evaluation options.
/// </summary>
public sealed class DefaultCriterion : BaseCriterion
{
	/// <summary>
	/// Gets the set of evaluation options for this criterion.
	/// </summary>
	public required IReadOnlyList<EvaluationOption> Options { get; init; }
}
