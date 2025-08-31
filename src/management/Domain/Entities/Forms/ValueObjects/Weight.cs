namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Non-negative percentage weight as an immutable value object.
/// </summary>
public sealed record Weight : IWeight
{
    private readonly ushort _bps;

    /// <summary>
    /// Initializes a new instance with the specified basis points.
    /// </summary>
    public Weight(ushort bps)
    {
        _bps = bps;
    }

    /// <summary>
    /// Returns the weight value as a percentage.
    /// </summary>
    public decimal Percent()
    {
        if (_bps > 10_000)
        {
            throw new InvalidDataException("Weight must be less than or equal to one hundred percent");
        }
        return _bps / 100m;
    }

    /// <summary>
    /// Returns the weight value in basis points.
    /// </summary>
    public ushort Bps()
    {
        if (_bps > 10_000)
        {
            throw new InvalidDataException("Weight basis points must be less than or equal to ten thousand");
        }
        return _bps;
    }
}
