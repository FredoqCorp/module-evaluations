namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;

/// <summary>
/// Short human-readable title for a criterion, limited to 100 characters.
/// </summary>
public readonly record struct CriterionTitle
{
    /// <summary>
    /// Creates criterion title ensuring it is non-empty and within the 100 character limit.
    /// </summary>
    /// <param name="text">Non-empty title not exceeding 100 characters.</param>
    /// <exception cref="ArgumentException">Thrown when title is empty, whitespace only, or exceeds 100 characters.</exception>
    public CriterionTitle(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(text.Length, 100);

        Text = text;
    }

    /// <summary>
    /// The title text of the criterion.
    /// </summary>
    public string Text { get; init; }    
}
