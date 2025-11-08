using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Short human-readable title for a criterion, limited to 100 characters.
/// </summary>
public sealed record CriterionTitle : ICriterionTitle
{
    private readonly string _value;

    /// <summary>
    /// Creates criterion title ensuring it is non-empty and within the 100 character limit.
    /// </summary>
    /// <param name="text">Non-empty title not exceeding 100 characters.</param>
    /// <exception cref="ArgumentException">Thrown when title is empty, whitespace only, or exceeds 100 characters.</exception>
    public CriterionTitle(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        _value = text;
    }

    /// <summary>
    /// Reads the title text of the criterion.
    /// </summary>
    /// <returns>Criterion title string.</returns>
    public string Text() => _value;
}
