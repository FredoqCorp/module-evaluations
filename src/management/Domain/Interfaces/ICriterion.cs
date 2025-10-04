using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a criterion that calculates its final score.
/// </summary>
public interface ICriterion
{
    /// <summary>
    /// Calculates the final score based on the selected rating.
    /// </summary>
    /// <returns>The criterion score if a rating is selected; otherwise, None.</returns>
    Option<CriterionScore> Score();
}
