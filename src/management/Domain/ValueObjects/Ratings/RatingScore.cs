namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores the numeric value associated with a rating option.
/// </summary>
public readonly record struct RatingScore
{
    /// <summary>
    /// Creates a numeric score while enforcing a positive domain.
    /// </summary>
    /// <param name="value">Numeric value defined by the scale type.</param>
    public RatingScore(ushort value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

        Value = value;
    }

    /// <summary>
    /// Numeric score stored for downstream calculations.
    /// </summary>
    public ushort Value { get; init; }
}
