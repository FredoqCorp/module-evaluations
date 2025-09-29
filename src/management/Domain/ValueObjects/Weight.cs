using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores a weight as basis points while supporting construction from percentages.
/// </summary>
public sealed record Weight : IWeight
{
    private readonly IBasisPoints _points;


    /// <summary>
    /// Initializes the weight from an explicit basis points value.
    /// </summary>
    /// <param name="points">Basis points representation of the weight.</param>
    public Weight(IBasisPoints points)
    {
        _points = points;
    }

    /// <summary>
    /// Initializes the weight from a percentage value using basis points conversion.
    /// </summary>
    /// <param name="percent">Percentage representation of the weight.</param>
    public Weight(IPercent percent)
        : this(percent.Basis())
    {
    }
    /// <summary>
    /// Returns a percentage representation derived from the stored basis points.
    /// </summary>
    /// <returns>Percent equivalent of the stored weight.</returns>
    public IPercent Percent()
    {
        return _points.Percent();
    }
}
