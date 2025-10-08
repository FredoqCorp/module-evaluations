namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a criterion that contributes through a weight.
/// </summary>
public interface IWeightedCriterion : ICriterion
{
    /// <summary>
    /// Returns the weight associated with the criterion.
    /// </summary>
    /// <returns>Weight assigned to the criterion.</returns>
    IWeight Weight();
}
