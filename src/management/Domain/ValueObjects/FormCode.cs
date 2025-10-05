namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that preserves the textual code of a form.
/// </summary>
public readonly record struct FormCode
{
    /// <summary>
    /// Initializes the value object with the provided token after validating basic integrity.
    /// </summary>
    /// <param name="token">Plain code token supplied by the caller.</param>
    public FormCode(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        Token = token.Trim();
    }

    /// <summary>
    /// Textual representation of the form code.
    /// </summary>
    public string Token { get; init; }
}
