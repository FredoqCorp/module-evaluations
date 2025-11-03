using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Ratings;

/// <summary>
/// Immutable decorator that validates the label text for rating options.
/// </summary>
public sealed class ValidRatingLabel : IRatingLabel
{
    private readonly IRatingLabel _original;

    /// <summary>
    /// Initializes the decorator with the original label source.
    /// </summary>
    /// <param name="original">Label source to validate.</param>
    public ValidRatingLabel(IRatingLabel original)
    {
        ArgumentNullException.ThrowIfNull(original);
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
