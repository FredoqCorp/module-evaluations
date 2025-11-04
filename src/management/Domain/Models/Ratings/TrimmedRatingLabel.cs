using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable decorator that trims the label text for a rating option.
/// </summary>
public sealed class TrimmedRatingLabel : IRatingLabel
{
    private readonly IRatingLabel _original;

    /// <summary>
    /// Initializes the decorator with the original label source.
    /// </summary>
    /// <param name="original">Label source to trim.</param>
    public TrimmedRatingLabel(IRatingLabel original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
