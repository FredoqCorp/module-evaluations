namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;

/// <summary>
/// Immutable value object that represents the final score of a criterion used in calculating the overall form score.
/// </summary>
public readonly record struct CriterionScore
{
    /// <summary> 
    /// The numeric score value.
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Creates a criterion score with the specified value.
    /// </summary>
    /// <param name="value">The numeric score value.</param>
    public CriterionScore(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        Value = value;
    }
}
