using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable description decorator that enforces description constraints.
/// </summary>
public sealed class ValidFormDescription : IFormDescription
{
    private readonly IFormDescription _original;

    /// <summary>
    /// Initializes the decorator with the original description source.
    /// </summary>
    /// <param name="original">Description source to validate.</param>
    public ValidFormDescription(IFormDescription original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var text = _original.Text();
        ArgumentNullException.ThrowIfNull(text);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 1000);
        return text;
    }
}
