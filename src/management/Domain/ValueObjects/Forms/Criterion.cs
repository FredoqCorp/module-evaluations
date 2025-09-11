using System.Collections.Immutable;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Domain layer value object representing an evaluation criterion with text and selectable options.
/// </summary>
public sealed record Criterion
{

    /// <summary>
    /// Initializes a new instance of the <see cref="Criterion"/> record with the specified criterion text and options.
    /// </summary>
    /// <param name="text">The criterion text value object.</param>
    /// <param name="options">The list of selectable options for this criterion.</param>
    public Criterion(CriterionText text, IImmutableList<Choice> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        Text = text;
        Options = options;
    }

    /// <summary>
    /// Returns the criterion text.
    /// </summary>
    public CriterionText Text { get; }

    /// <summary>
    /// Returns the list of selectable options for this criterion.
    /// </summary>
    public IImmutableList<Choice> Options { get; }


}
