using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Enums.Forms;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Form;

/// <summary>
/// Represents an evaluation form with its properties and criteria.
/// </summary>
public sealed class EvaluationForm
{
    /// <summary>
    /// Gets the unique identifier of the evaluation form.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the title of the evaluation form.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Gets the code of the evaluation form.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Gets the description of the evaluation form.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the status of the evaluation form.
    /// </summary>
    public EvaluationFormStatus Status { get; init; } = EvaluationFormStatus.Draft;

    /// <summary>
    /// Gets the username or identifier of the user who created the evaluation form.
    /// </summary>
    public required string CreatedBy { get; init; }

    /// <summary>
    /// Gets the date and time when the evaluation form was created.
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the username or identifier of the user who changed the status of the evaluation form.
    /// </summary>
    public string? StatusChangedBy { get; init; }

    /// <summary>
    /// Gets the date and time when the status of the evaluation form was changed.
    /// </summary>
    public DateTime? StatusChangedAt { get; init; }

    /// <summary>
    /// Gets the username or identifier of the user who last modified the evaluation form.
    /// </summary>
    public string? ModifiedBy { get; init; }

    /// <summary>
    /// Gets the date and time when the evaluation form was last modified.
    /// </summary>
    public DateTime? ModifiedAt { get; init; }

    /// <summary>
    /// Gets the date and time from which the evaluation form is valid.
    /// </summary>
    public DateTime? ValidFrom { get; init; }

    /// <summary>
    /// Gets the date and time until which the evaluation form is valid.
    /// </summary>
    public DateTime? ValidUntil { get; init; }

    /// <summary>
    /// Gets the calculation rule used for the evaluation form.
    /// </summary>
    public FormCalculationRule CalculationRule { get; init; }

    /// <summary>
    /// Gets the keywords associated with the evaluation form.
    /// </summary>
    public IReadOnlyList<string>? FormKeywords { get; init; }

    /// <summary>
    /// Gets the list of criteria associated with the evaluation form.
    /// </summary>
    public IReadOnlyList<BaseCriterion>? FormCriteria { get; init; }

}
