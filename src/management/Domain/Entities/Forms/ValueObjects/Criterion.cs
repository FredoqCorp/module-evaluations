using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Domain layer value object representing an evaluation criterion with text and selectable options.
/// </summary>
public sealed record Criterion : ICriterion
{
    private readonly ICriterionText _text;
    private readonly IImmutableList<IChoice> _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="Criterion"/> record with the specified criterion text and options.
    /// </summary>
    /// <param name="text">The criterion text value object.</param>
    /// <param name="options">The list of selectable options for this criterion.</param>
    public Criterion(ICriterionText text, IImmutableList<IChoice> options)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(options);

        _text = text;
        _options = options;
    }

    /// <summary>
    /// Returns the human readable title string.
    /// </summary>
    public string Title()
    {
        return _text.Title();
    }

    /// <summary>
    /// Returns the detailed description string.
    /// </summary>
    public string Description()
    {
        return _text.Description();
    }

    /// <summary>
    /// Returns the list of available options for scoring.
    /// </summary>
    public IImmutableList<IChoice> Options()
    {
        return _options;
    }
}
