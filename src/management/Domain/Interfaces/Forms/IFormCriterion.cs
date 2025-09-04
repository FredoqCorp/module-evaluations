namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Contract for a criterion positioned within a form or group.
/// </summary>
public interface IFormCriterion
{
    /// <summary>
    /// Returns the stable identifier of this positioned criterion.
    /// </summary>
    IId Id();
    /// <summary>
    /// Returns the domain criterion value object.
    /// </summary>
    ICriterion Criterion();

    /// <summary>
    /// Returns the display order index value object.
    /// </summary>
    IOrderIndex Order();

}
