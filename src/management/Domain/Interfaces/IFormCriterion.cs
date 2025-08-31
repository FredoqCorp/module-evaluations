namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Contract for a criterion positioned within a form or group.
/// </summary>
public interface IFormCriterion
{
    /// <summary>
    /// Returns the domain criterion value object.
    /// </summary>
    ICriterion Criterion();

    /// <summary>
    /// Returns the display order index value object.
    /// </summary>
    IOrderIndex Order();

    /// <summary>
    /// Returns the weight when used.
    /// </summary>
    IWeight Weight();
}
