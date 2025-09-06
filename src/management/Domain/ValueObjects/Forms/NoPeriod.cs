using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Null Object implementation of IPeriod representing no validity constraints.
/// </summary>
public sealed record NoPeriod : IPeriod
{
    /// <summary>
    /// Initializes a new no-validity period value object.
    /// </summary>
    public NoPeriod()
    {
    }

    /// <summary>
    /// Returns minimal start value.
    /// </summary>
    public DateTime Start() => DateTime.MinValue;

    /// <summary>
    /// Returns maximal finish value.
    /// </summary>
    public DateTime Finish() => DateTime.MaxValue;
}
