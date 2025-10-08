namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Validity;

/// <summary>
/// Immutable value object representing the inclusive validity end moment.
/// </summary>
public readonly record struct ValidityEnd
{
    /// <summary>
    /// Initializes the end moment using the provided value.
    /// </summary>
    /// <param name="value">Moment that closes the validity window.</param>
    public ValidityEnd(DateTime value)
    {
        Value = value;
    }

    /// <summary>
    /// Underlying date and time of the validity end.
    /// </summary>
    public DateTime Value { get; }
}
