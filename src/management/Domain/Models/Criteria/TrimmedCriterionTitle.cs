using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Immutable decorator that trims whitespace from the original criterion title.
/// </summary>
public sealed class TrimmedCriterionTitle : ICriterionTitle
{
    private readonly ICriterionTitle _original;

    /// <summary>
    /// Initializes the decorator with the original title source.
    /// </summary>
    /// <param name="original">Title source to trim.</param>
    public TrimmedCriterionTitle(ICriterionTitle original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public string Text() => _original.Text().Trim();
}
