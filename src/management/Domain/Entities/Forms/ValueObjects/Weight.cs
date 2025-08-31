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
    public decimal Percent() => _bps / 100m;

    /// <summary>
    /// Returns the weight value in basis points.
    /// </summary>
    public ushort Bps() => _bps;
}
