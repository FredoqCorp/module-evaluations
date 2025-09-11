namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Domain layer value object that encapsulates a criterion title and description as immutable text.
/// </summary>
public readonly record struct CriterionText
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CriterionText"/> value object with the specified title and description.
    /// </summary>
    /// <param name="title">The human readable title of the criterion.</param>
    /// <param name="description">The detailed description of the criterion.</param>
    public CriterionText(string title, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(description);

        Title = title.Trim();
        Description = description.Trim();
    }

    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    public string Description { get; }
}
