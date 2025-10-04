using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities;

/// <summary>
/// Represents a criterion used for evaluations.
/// </summary>
public sealed class Criterion : ICriterion
{
    private readonly CriterionId _id;
    private readonly CriterionText _text;
    private readonly CriterionTitle _title;
    private readonly IRatingOptions _ratingOptions;

    /// <summary>
    /// Creates a new criterion with the specified id, text, and title.
    /// </summary>
    /// <param name="id">Unique identifier for the criterion.</param>
    /// <param name="text">Text description of the criterion.</param>
    /// <param name="title">Title of the criterion.</param>
    /// <param name="ratingOptions">Rating options associated with the criterion.</param>
    public Criterion(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions)
    {
        _id = id;
        _text = text;
        _title = title;
        _ratingOptions = ratingOptions;
    }

    /// <summary>
    /// Calculates the final score based on the selected rating.
    /// </summary>
    /// <returns>The criterion score if a rating is selected; otherwise, None.</returns>
    public Option<CriterionScore> Score() => _ratingOptions
            .Score()
            .Map(score => new CriterionScore(score.Value));
}
