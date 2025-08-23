namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;



/// <summary>
/// Represents a service code value object.
/// </summary>
public sealed record ServiceCode
{
    /// <summary>
    /// Gets the service code value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceCode"/> record with the specified value.
    /// </summary>
    /// <param name="value">The service code value.</param>
    /// <exception cref="ArgumentException">Thrown when the value is null, empty, or exceeds 50 characters.</exception>
    public ServiceCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("ServiceCode is required.", nameof(value));
        }

        value = value.Trim();
        if (value.Length > 50)
        {
            throw new ArgumentException("ServiceCode is too long (max 50).", nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Returns the string representation of the service code.
    /// </summary>
    /// <returns>The service code value as a string.</returns>
    public override string ToString() => Value;
}
