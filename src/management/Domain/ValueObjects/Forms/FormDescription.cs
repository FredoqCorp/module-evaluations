namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Immutable value object that stores the optional form description text.
/// </summary>
public readonly record struct FormDescription
{
    /// <summary>
    /// Creates a description from provided text while preventing null.
    /// </summary>
    /// <param name="value">Description text provided by the caller.</param>
    public FormDescription(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var trimmed = value.Trim();
        ArgumentOutOfRangeException.ThrowIfGreaterThan(trimmed.Length, 1000);

        Value = trimmed;
    }

    /// <summary>
    /// Original description text value.
    /// </summary>
    public string Value { get; init; }
}
