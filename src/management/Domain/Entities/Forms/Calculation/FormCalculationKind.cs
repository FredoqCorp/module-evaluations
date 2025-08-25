namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Calculation;

/// <summary>
/// Describes how total score should be calculated for a form.
/// Only the selection is stored on the form; execution occurs in a separate "form run" object.
/// </summary>
public enum FormCalculationKind
{
    /// <summary>
    /// Simple arithmetic mean of all criterion scores.
    /// </summary>
    ArithmeticMean = 0,

    /// <summary>
    /// Weighted mean using group and criterion weights (sum to 100%).
    /// </summary>
    WeightedMean = 1,
}
