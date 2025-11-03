using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Immutable decorator that enforces invariant rules for a criterion title.
/// </summary>
public sealed class ValidCriterionTitle : ICriterionTitle
{
    private readonly ICriterionTitle _original;

    /// <summary>
    /// Initializes the decorator with the original title source.
    /// </summary>
    /// <param name="original">Title source to validate.</param>
    public ValidCriterionTitle(ICriterionTitle original)
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
