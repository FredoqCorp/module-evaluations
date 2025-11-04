using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Forms;

/// <summary>
/// Immutable code decorator that enforces code constraints.
/// </summary>
public sealed class ValidFormCode : IFormCode
{
    private readonly IFormCode _original;

    /// <summary>
    /// Initializes the decorator with the original code source.
    /// </summary>
    /// <param name="original">Code source to validate.</param>
    public ValidFormCode(IFormCode original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var token = _original.Text();
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        return token;
    }
}
