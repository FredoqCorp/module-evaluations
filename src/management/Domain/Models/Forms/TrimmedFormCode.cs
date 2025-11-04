using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable code decorator that trims whitespace from the original token.
/// </summary>
public sealed class TrimmedFormCode : IFormCode
{
    private readonly IFormCode _original;

    /// <summary>
    /// Initializes the decorator with the original code source.
    /// </summary>
    /// <param name="original">Code source to trim.</param>
    public TrimmedFormCode(IFormCode original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
