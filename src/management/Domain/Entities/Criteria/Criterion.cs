using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Criterion is a question used to evaluate a fact and choose a score via predefined options.
/// </summary>
public sealed class Criterion
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Textual content (title + description).
    /// </summary>
    public required CriterionText Text { get; init; }

    /// <summary>
    /// Available options to choose a score from.
    /// </summary>
    public required IReadOnlyList<Choice> Options { get; init; }
}
