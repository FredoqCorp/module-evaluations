using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable value object that stores the human readable form name with trimmed whitespace.
/// </summary>
public record TrimmedFormName : IFormName
{
    private readonly IFormName _original;

    /// <summary>
    /// Initializes trimmed form name from the original form name.
    /// </summary>
    /// <param name="original">Form name source to trim.</param>
    public TrimmedFormName(IFormName original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
