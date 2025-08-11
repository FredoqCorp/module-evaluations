using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;

/// <summary>
/// Represents an evaluation criterion that is automatically determined by an <see cref="AutomaticParameter"/>.
/// </summary>
public sealed class AutomaticCriterion : BaseCriterion
{
    /// <summary>
    /// Gets the automatic parameter associated with this criterion.
    /// </summary>
    public required AutomaticParameter AutomaticParameter { get; init; }

    /// <summary>
    /// Gets the list of evaluation options for this criterion.
    /// </summary>
    public required IReadOnlyList<EvaluationOption> Options { get; init; }
}
