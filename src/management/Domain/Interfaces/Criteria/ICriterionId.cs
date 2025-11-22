namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

/// <summary>
/// Represents a criterion identifier.
/// </summary>
public interface ICriterionId
{
    /// <summary>
    /// Gets the unique identifier value.
    /// </summary>
    Guid Value();
}
