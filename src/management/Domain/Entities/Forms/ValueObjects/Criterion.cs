using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Domain layer entity representing a single evaluation criterion with identity, text and selectable options.
/// </summary>
public sealed record Criterion : ICriterion
{
    private readonly ICriterionText _text;
    private readonly IReadOnlyList<IChoice> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="Criterion"/> record with the specified criterion text and options.
    /// </summary>
    /// <param name="text">The criterion text value object.</param>
    /// <param name="options">The list of selectable options for this criterion.</param>
    public Criterion(ICriterionText text, IReadOnlyList<IChoice> options)
    {
        _text = text;
        _options = options;
    }

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
