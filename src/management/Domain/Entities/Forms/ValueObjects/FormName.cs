using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;

/// <summary>
/// Name/title of a form as an immutable value object.
/// </summary>
public sealed record FormName : IFormName
{
    private readonly string _value;

    /// <summary>
    /// Creates a form name with a non-null raw string.
    /// </summary>
    public FormName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _value = value;
    }

    /// <summary>
    /// Returns the raw name string.
    /// </summary>
    public string Name()
    {
        if (string.IsNullOrWhiteSpace(_value))
        {
            throw new InvalidDataException("Name must not be empty");
        }
        return _value;
    }

    /// <summary>
    /// Returns the raw name string representation.
    /// </summary>
    public override string ToString() => _value;
}
