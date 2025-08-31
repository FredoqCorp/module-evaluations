using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Domain layer entity representing a single evaluation criterion with identity, text and selectable options.
/// </summary>
public sealed class Criterion(IId id, ICriterionText text, IReadOnlyList<IChoice> options) : ICriterion
{
    private readonly IId _id = id;
    private readonly ICriterionText _text = text;
    private readonly IReadOnlyList<IChoice> _options = options;

    /// <summary>
    /// Returns the unique identifier of this criterion.
    /// </summary>
    public IId Id() => _id;

    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    public string Title() => _text.Title();

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    public string Description() => _text.Description();

    /// <summary>
    /// Returns the list of available options for scoring.
    /// </summary>
    public IReadOnlyList<IChoice> Options() => _options;
}
