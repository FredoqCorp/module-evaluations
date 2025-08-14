using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Enums.Forms;

namespace CascVel.Module.Evaluations.Management.Application.Models;

/// <summary>
/// Represents a set of changes to be applied to an evaluation form.
/// </summary>
public sealed record EvaluationFormChangeSet
{
    /// <summary>
    /// Title of the evaluation
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Code of the evaluation form.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Description of the evaluation form.
    /// </summary>
    public required string? Description { get; init; }

    /// <summary>
    /// The date and time from which the evaluation form is valid.
    /// </summary>
    public required DateTime? ValidFrom { get; init; }

    /// <summary>
    /// The date and time until which the evaluation form is valid.
    /// </summary>
    public required DateTime? ValidUntil { get; init; }

    /// <summary>
    /// The calculation rule used for the evaluation form.
    /// </summary>
    public required FormCalculationRule CalculationRule { get; init; }

    /// <summary>
    /// The keywords associated with the evaluation form.
    /// </summary>
    public required IReadOnlyList<string>? FormKeywords { get; init; }

    /// <summary>
    /// The list of criteria associated with the evaluation form.
    /// </summary>
    public required IReadOnlyList<BaseCriterion>? FormCriteria { get; init; }

    /// <summary>
    /// The user who last modified the evaluation form.
    /// </summary>
    public required string ModifiedBy { get; init; }
}
