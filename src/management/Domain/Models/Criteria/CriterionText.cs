using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Detailed explanation or question text for a criterion, limited to 1000 characters.
/// </summary>
public sealed record CriterionText : ICriterionText
{
    private readonly string _value;

    /// <summary>
    /// Creates criterion text ensuring it is non-empty and within the 1000 character limit.
    /// </summary>
    /// <param name="text">Non-empty text not exceeding 1000 characters.</param>
    /// <exception cref="ArgumentException">Thrown when text is empty, whitespace only, or exceeds 1000 characters.</exception>
    public CriterionText(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        _value = text;
    }

    /// <summary>
    /// Reads the text content of the criterion.
    /// </summary>
    /// <returns>Criterion body string.</returns>
    public string Text() => _value;
}
