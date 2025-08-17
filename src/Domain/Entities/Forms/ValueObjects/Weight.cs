namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Non-negative percentage weight. For groups/criteria in the form.
/// </summary>
public sealed record Weight
{
    /// <summary>
    /// Value in percents, 0..100. Total must sum to 100% in context.
    /// </summary>
    private decimal _percent { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Weight"/> record with the specified percentage value.
    /// </summary>
    /// <param name="percent">The weight value in percent (0..100).</param>
    public Weight(decimal percent)
    {
        _percent = percent;
    }

    /// <summary>
    /// Gets the weight value in percent, rounded to two decimal places.
    /// Throws <see cref="InvalidDataException"/> if the value is not between 0 and 100.
    /// </summary>
    /// <returns>The weight value in percent.</returns>
    public decimal Percent()
    {
        if (_percent is < 0m or > 100m)
        {
            throw new InvalidDataException("Weight percent must be between 0..100%");
        }
        return decimal.Round(_percent, 2, MidpointRounding.AwayFromZero);
    }

    /// <summary>
    /// Gets the weight value in basis points (1% = 100 bps), rounded to the nearest integer.
    /// Throws <see cref="InvalidDataException"/> if the value is not between 0 and 100.
    /// </summary>
    /// <returns>The weight value in basis points as an unsigned short.</returns>
    public ushort Bps()
    {
        if (_percent is < 0m or > 100m)
        {
            throw new InvalidDataException("Weight percent must be between 0..100%");
        }
        return checked((ushort)decimal.Round(_percent * 100m, 0, MidpointRounding.AwayFromZero));
    }
}
