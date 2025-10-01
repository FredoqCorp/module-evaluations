namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing the inclusive validity start moment.
/// </summary>
public readonly record struct ValidityStart
{
    /// <summary>
    /// Initializes the start moment using the provided value.
    /// </summary>
    /// <param name="value">Moment that opens the validity window.</param>
    public ValidityStart(DateTime value)
    {
        Value = value;
    }

    /// <summary>
    /// Underlying date and time of the validity start.
    /// </summary>
    public DateTime Value { get; init; }
}
