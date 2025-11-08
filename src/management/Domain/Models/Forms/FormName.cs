using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable value object that stores the human readable form name.
/// </summary>
public sealed record FormName : IFormName
{
    private readonly string _text;

    /// <summary>
    /// Creates a form name from provided text while preventing null.
    /// </summary>
    /// <param name="value">Name text provided by the caller.</param>
    public FormName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        _text = value;
    }

    /// <summary>
    /// Reads the form name text.
    /// </summary>
    /// <returns>Form name string.</returns>
    public string Text() => _text;
}
