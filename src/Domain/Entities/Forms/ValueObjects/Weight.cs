namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Non-negative percentage weight. For groups/criteria in the form.
/// </summary>
public sealed record Weight
{
    private readonly ushort _bps;


    /// <summary>
    /// Initializes a new instance of the <see cref="Weight"/> value object with the specified basis points.
    /// </summary>
    /// <param name="bps">The weight value in basis points (1% = 100 bps).</param>
    public Weight(ushort bps)
    {
        _bps = bps;
    }

    /// <summary>
    /// Gets the weight value as a percentage.
    /// </summary>
    /// <returns>The weight value as a decimal percentage.</returns>
    public decimal Percent() => _bps / 100m;

    /// <summary>
    /// Gets the weight value in basis points (bps).
    /// </summary>
    /// <returns>The weight value in basis points.</returns>
    public ushort Bps() => _bps;
}
