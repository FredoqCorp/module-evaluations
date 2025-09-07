namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Name/title of a form.
/// </summary>
public readonly record struct FormName
{

    /// <summary>
    /// Creates a form name with a non-null raw string.
    /// </summary>
    public FormName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    /// <summary>
    /// Returns the raw name string.
    /// </summary>
    public string Value { get; }
}
