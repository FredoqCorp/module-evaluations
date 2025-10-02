using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Immutable collection of rating options.
/// </summary>
public sealed record RatingOptions : IRatingOptions
{
    private readonly ImmutableList<IRatingOption> _options;
    private readonly Option<IRatingOption> _selectedOption;

    /// <summary>
    /// Creates a collection of rating options from the provided enumerable without accepting null.
    /// </summary>
    /// <param name="options">Enumerable of rating options.</param>
    public RatingOptions(IEnumerable<IRatingOption> options)
    {
        _options = [.. options];

        _selectedOption = Option.None<IRatingOption>();
    }

    /// <summary>
    /// Private constructor for creating a new instance with a selected option.
    /// </summary>
    private RatingOptions(ImmutableList<IRatingOption> options, IRatingOption selectedOption)
    {
        _options = options;
        _selectedOption = Option.Of(selectedOption);
    }

    /// <summary>
    /// Selects a rating option by score from the available options.
    /// </summary>
    /// <param name="score">The score to select.</param>
    /// <returns>A new instance with the selected option.</returns>
    /// <exception cref="Exceptions.ScoreNotFoundException">Thrown when the score is not found in the available options.</exception>
    public IRatingOptions WithSelectedScore(RatingScore score)
    {
        var matchingOption = _options.FirstOrDefault(opt => opt.Matches(score)) ?? throw new Exceptions.ScoreNotFoundException(score);
        return new RatingOptions(_options, matchingOption);
    }

}
