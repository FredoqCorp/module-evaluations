using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Globally-unique code of a form as an immutable value object.
/// </summary>
public sealed record FormCode : IFormCode
{
    private readonly string _value;

    /// <summary>
    /// Creates a form code with a non-null raw string.
    /// </summary>
    public FormCode(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Returns the raw code string.
    /// </summary>
    public string Code()
    {
        if (string.IsNullOrWhiteSpace(_value))
        {
            throw new InvalidDataException("Code must not be empty");
        }
        return _value;
    }

    /// <summary>
    /// Returns the raw code string representation.
    /// </summary>
    public override string ToString() => _value;
}
