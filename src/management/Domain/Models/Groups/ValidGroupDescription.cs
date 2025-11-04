using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Immutable decorator that enforces invariant rules for a group description.
/// </summary>
public sealed class ValidGroupDescription : IGroupDescription
{
    private readonly IGroupDescription _original;

    /// <summary>
    /// Initializes the decorator with the original description source.
    /// </summary>
    /// <param name="original">Description source to validate.</param>
    public ValidGroupDescription(IGroupDescription original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var value = _original.Text();
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, 1000);
        return value;
    }
}
