namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Null Object implementation of IWeight representing zero weight.
/// </summary>
public sealed record ZeroWeight : IWeight
{
    /// <summary>
    /// Initializes a new zero weight value object.
    /// </summary>
    public ZeroWeight()
    {
    }

    /// <summary>
    /// Returns weight value as zero percent.
    /// </summary>
    public decimal Percent() => 0m;

    /// <summary>
    /// Returns weight value as zero bps.
    /// </summary>
    public ushort Bps() => 0;
}
