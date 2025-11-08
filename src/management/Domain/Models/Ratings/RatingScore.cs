using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable value object that stores the numeric value associated with a rating option.
/// </summary>
public sealed record RatingScore : IRatingScore
{
    private readonly ushort _amount;

    /// <summary>
    /// Creates a numeric score while enforcing a positive domain.
    /// </summary>
    /// <param name="value">Numeric value defined by the scale type.</param>
    public RatingScore(ushort value)
    {
        _amount = value;
    }

    /// <summary>
    /// Reads the numeric score.
    /// </summary>
    /// <returns>Rating score value.</returns>
    public int Value() => _amount;
}
