using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores a percentage with basis points precision.
/// </summary>
public sealed record Percent: IPercent
{
    private readonly decimal _value;

    /// <summary>
    /// Initializes the percentage while enforcing range and granularity constraints.
    /// </summary>
    /// <param name="value">Decimal representation of the percentage.</param>
    public Percent(decimal value)
    {

        _value = value;
    }

    /// <summary>
    /// Returns a basis points representation derived from the stored percentage.
    /// </summary>
    /// <returns>Basis points value equivalent to the stored percentage.</returns>
    public IBasisPoints Basis()
    {
        if (_value < decimal.Zero)
        {
            throw new InvalidDataException("Percent must be non negative");
        }

        if (_value > 100m)
        {
            throw new InvalidDataException("Percent must not exceed 100");
        }

        var scaled = decimal.Truncate(_value * 100m);
        return new BasisPoints((ushort)scaled);
    }
}
