using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores a weight in basis points ensuring boundaries suitable for scoring rules.
/// </summary>
public sealed record BasisPoints : IBasisPoints
{
    private readonly ushort _value;

    /// <summary>
    /// Initializes the value object with a given basis points amount within the inclusive range 0..10000.
    /// </summary>
    /// <param name="value">Amount of basis points.</param>
    public BasisPoints(ushort value)
    {
        _value = value;
    }

    /// <summary>
    /// Returns a percent representation computed from the stored basis points.
    /// </summary>
    /// <returns>Percent value equivalent to the stored basis points.</returns>
    public IPercent Percent()
    {
        if (_value > 10000)
        {
            throw new InvalidDataException("Basis points must not exceed 10000");
        }

        return new Percent(_value / 100m);
    }
}
