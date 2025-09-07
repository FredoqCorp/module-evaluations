using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Globally-unique code of a form as an immutable value object.
/// </summary>
public readonly record struct FormCode
{
    /// <summary>
    /// Creates a form code with a non-null raw string.
    /// </summary>
    public FormCode(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    /// <summary>
    /// Returns the raw code string.
    /// </summary>
    public string Value { get; }
}
