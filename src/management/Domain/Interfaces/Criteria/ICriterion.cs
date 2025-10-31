namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Behavioral contract for a single criterion.
/// </summary>
public interface ICriterion
{
    /// <summary>
    /// Validates the internal consistency of the criterion.
    /// </summary>
    void Validate();

}
