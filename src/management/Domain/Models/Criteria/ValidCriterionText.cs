using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Immutable decorator that enforces invariant rules for a criterion text.
/// </summary>
public sealed class ValidCriterionText : ICriterionText
{
    private readonly ICriterionText _original;

    /// <summary>
    /// Initializes the decorator with the original text source.
    /// </summary>
    /// <param name="original">Text source to validate.</param>
    public ValidCriterionText(ICriterionText original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text()
    {
        var value = _original.Text();
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, 1000);
        return value;
    }
}
