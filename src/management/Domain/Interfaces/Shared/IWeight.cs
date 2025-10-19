using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

/// <summary>
/// Behavioral contract for a weight that exposes both basis points and percent views.
/// </summary>
public interface IWeight
{

    /// <summary>
    /// Returns the percent representation of the weight.
    /// </summary>
    /// <returns>Percent equivalent of the weight.</returns>
    IPercent Percent();

    /// <summary>
    /// Applies the weight to a criterion score.
    /// </summary>
    /// <param name="score">The criterion score to apply the weight to.</param>
    /// <returns>A new criterion score with the weight applied.</returns>
    CriterionScore Weighted(CriterionScore score);
}
