using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Immutable decorator that trims whitespace from the original group description.
/// </summary>
public sealed class TrimmedGroupDescription : IGroupDescription
{
    private readonly IGroupDescription _original;

    /// <summary>
    /// Initializes the decorator with the original description source.
    /// </summary>
    /// <param name="original">Description source to trim.</param>
    public TrimmedGroupDescription(IGroupDescription original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
