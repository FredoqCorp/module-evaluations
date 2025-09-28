using System;
using System.IO;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that preserves the textual code of a form.
/// </summary>
public readonly record struct FormCode
{
    private readonly string _token;

    /// <summary>
    /// Initializes the value object with the provided token after validating basic integrity.
    /// </summary>
    /// <param name="token">Plain code token supplied by the caller.</param>
    public FormCode(string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(token);

        _token = token;
    }

    /// <summary>
    /// Returns the token representation for diagnostics and persistence mappings.
    /// </summary>
    /// <returns>String token matching the domain invariant.</returns>
    public override string ToString()
    {
        return _token;
    }
}
