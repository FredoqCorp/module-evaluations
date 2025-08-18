namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;


/// <summary>
/// Represents a value object for basis points of a percent (0..100%), with rounding policy.
/// </summary>
public sealed record BpsOfPercent
{
    private readonly decimal _percent;
    private readonly MidpointRounding _policy;

    /// <summary>
    /// Initializes a new instance of the <see cref="BpsOfPercent"/> record with the specified percent and rounding policy.
    /// </summary>
    /// <param name="percent">The percent value (0..100) to represent as basis points.</param>
    /// <param name="policy">The rounding policy to use when converting to basis points. Defaults to <see cref="MidpointRounding.AwayFromZero"/>.</param>
    public BpsOfPercent(decimal percent, MidpointRounding policy = MidpointRounding.AwayFromZero)
    {
        _percent = percent;
        _policy = policy;
    }

    /// <summary>
    /// Validates 0..100, rounds to nearest bps, returns canonical ushort.
    /// </summary>
    public ushort Value()
    {
        if (_percent is < 0m or > 100m)
        {
            throw new InvalidDataException("Weight percent must be between 0..100%.");
        }

        decimal bps = decimal.Round(_percent * 100m, 0, _policy);
        return checked((ushort)bps);
    }
}
