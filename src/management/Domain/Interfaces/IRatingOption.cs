using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Behavioral contract for a single rating option within a scale.
/// </summary>
public interface IRatingOption
{
    /// <summary>
    /// Returns the numeric score associated with the option.
    /// </summary>
    /// <returns>Value object that contains the numeric score.</returns>
    RatingScore Score();

    /// <summary>
    /// Returns the label shown to form designers and evaluators.
    /// </summary>
    /// <returns>Value object that stores the configured label.</returns>
    RatingLabel Label();

    /// <summary>
    /// Returns the optional annotation that explains the option.
    /// </summary>
    /// <returns>Value object with optional annotation content.</returns>
    RatingAnnotation Annotation();
}
