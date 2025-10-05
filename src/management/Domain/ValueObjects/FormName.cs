namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable value object that stores the human readable form name.
/// </summary>
public readonly record struct FormName
{
    /// <summary>
    /// Creates a form name from provided text while preventing null.
    /// </summary>
    /// <param name="value">Name text provided by the caller.</param>
    public FormName(string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(value);

        Value = value.Trim();
    }

    /// <summary>
    /// Original name text value.
    /// </summary>
    public string Value { get; init; }
}

