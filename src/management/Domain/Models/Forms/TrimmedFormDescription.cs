using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable description decorator that trims whitespace from the original value.
/// </summary>
public sealed class TrimmedFormDescription : IFormDescription
{
    private readonly IFormDescription _original;

    /// <summary>
    /// Initializes the decorator with the original description source.
    /// </summary>
    /// <param name="original">Description source to trim.</param>
    public TrimmedFormDescription(IFormDescription original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
