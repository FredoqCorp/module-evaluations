namespace CascVel.Modules.Evaluations.Management.Domain.Models.Identity;

/// <summary>
/// Unique identifier for a module user extracted from authentication token.
/// </summary>
/// <remarks>
/// Uses string representation to support various identity provider formats
/// (GUID strings, numeric IDs, custom identifiers, etc.).
/// </remarks>
public readonly record struct UserId
{
    /// <summary>
    /// Creates user identifier ensuring it is not null or whitespace.
    /// </summary>
    /// <param name="value">Non-empty string representing the user identifier.</param>
    /// <exception cref="ArgumentException">Thrown when value is null, empty, or whitespace.</exception>
    public UserId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }

    /// <summary>
    /// Underlying string value representing the user identifier.
    /// </summary>
    public string Value { get; init; }
}
