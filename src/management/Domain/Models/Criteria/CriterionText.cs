namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Detailed explanation or question text for a criterion, limited to 1000 characters.
/// </summary>
public readonly record struct CriterionText
{
    /// <summary>
    /// Creates criterion text ensuring it is non-empty and within the 1000 character limit.
    /// </summary>
    /// <param name="text">Non-empty text not exceeding 1000 characters.</param>
    /// <exception cref="ArgumentException">Thrown when text is empty, whitespace only, or exceeds 1000 characters.</exception>
    public CriterionText(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 1000);

        Text = text;
    }

    /// <summary>
    /// The text content of the criterion.
    /// </summary>
    public string Text { get; init; }
}
