using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Immutable decorator that enforces invariant rules for a group title.
/// </summary>
public sealed class ValidGroupTitle : IGroupTitle
{
    private readonly IGroupTitle _original;

    /// <summary>
    /// Initializes the decorator with the original title source.
    /// </summary>
    /// <param name="original">Title source to validate.</param>
    public ValidGroupTitle(IGroupTitle original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var value = _original.Text();
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, 100);
        return value;
    }
}
