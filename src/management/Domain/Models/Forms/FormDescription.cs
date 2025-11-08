using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable value object that stores the optional form description text.
/// </summary>
public sealed record FormDescription : IFormDescription
{
    private readonly string _text;

    /// <summary>
    /// Creates a description from provided text while preventing null.
    /// </summary>
    /// <param name="value">Description text provided by the caller.</param>
    public FormDescription(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        _text = value;
    }

    /// <summary>
    /// Reads the form description text.
    /// </summary>
    /// <returns>Form description string.</returns>
    public string Text() => _text;
}
