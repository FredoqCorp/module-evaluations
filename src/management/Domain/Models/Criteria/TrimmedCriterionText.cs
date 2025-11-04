using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Immutable decorator that trims whitespace from the original criterion text.
/// </summary>
public sealed class TrimmedCriterionText : ICriterionText
{
    private readonly ICriterionText _original;

    /// <summary>
    /// Initializes the decorator with the original text source.
    /// </summary>
    /// <param name="original">Text source to trim.</param>
    public TrimmedCriterionText(ICriterionText original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
