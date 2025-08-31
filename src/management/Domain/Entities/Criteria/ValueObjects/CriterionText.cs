namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Domain layer value object that encapsulates a criterion title and description as immutable text.
/// </summary>
public sealed record CriterionText(string name, string text) : ICriterionText
{
    private readonly string _title = name;
    private readonly string _description = text;

    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    public string Title() => _title;

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    public string Description() => _description;
}
