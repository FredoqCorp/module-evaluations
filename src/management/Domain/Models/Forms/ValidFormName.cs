using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable value object that stores the human readable form name with validation.
/// </summary>
public record ValidFormName : IFormName
{
    private readonly IFormName _original;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidFormName"/> class.
    /// </summary>
    /// <param name="original">The original form name.</param>
    public ValidFormName(IFormName original)
    {
        _original = original;
    }


    /// <inheritdoc />
    public string Text()
    {
        var originalValue = _original.Text();

        ArgumentOutOfRangeException.ThrowIfGreaterThan(originalValue.Length, 100);

        return originalValue;
    }
}
