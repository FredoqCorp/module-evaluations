using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// Immutable tag decorator that enforces tag constraints.
/// </summary>
public sealed record ValidTag : ITag
{
    private readonly ITag _original;

    /// <summary>
    /// Initializes the decorator with the original tag source.
    /// </summary>
    /// <param name="original">Tag source to validate.</param>
    public ValidTag(ITag original)
    {
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var text = _original.Text();
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        return text;
    }
}
