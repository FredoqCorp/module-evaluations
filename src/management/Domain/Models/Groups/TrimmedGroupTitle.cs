using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Immutable decorator that trims whitespace from the original group title.
/// </summary>
public sealed class TrimmedGroupTitle : IGroupTitle
{
    private readonly IGroupTitle _original;

    /// <summary>
    /// Initializes the decorator with the original title source.
    /// </summary>
    /// <param name="original">Title source to trim.</param>
    public TrimmedGroupTitle(IGroupTitle original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
