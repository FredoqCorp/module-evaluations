namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Domain layer value object that encapsulates a criterion title and description as immutable text.
/// </summary>
public sealed record CriterionText : ICriterionText
{
    private readonly string _title;
    private readonly string _description;

    /// <summary>
    /// Initializes a new instance of the <see cref="CriterionText"/> value object with the specified title and description.
    /// </summary>
    /// <param name="title">The human readable title of the criterion.</param>
    /// <param name="description">The detailed description of the criterion.</param>
    public CriterionText(string title, string description)
    {
        _title = title;
        _description = description;
    }

    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    public string Title() => _title;

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    public string Description() => _description;
}
