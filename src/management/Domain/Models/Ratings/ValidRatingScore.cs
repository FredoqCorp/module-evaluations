using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable decorator that validates the numeric score for rating options.
/// </summary>
public sealed class ValidRatingScore : IRatingScore
{
    private readonly IRatingScore _original;

    /// <summary>
    /// Initializes the decorator with the original score source.
    /// </summary>
    /// <param name="original">Score source to validate.</param>
    public ValidRatingScore(IRatingScore original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public int Value()
    {
        var value = _original.Value();
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 5);
        return value;
    }
}
