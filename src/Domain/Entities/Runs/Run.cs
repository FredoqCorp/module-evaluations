using CascVel.Module.Evaluations.Management.Domain.Enums.Runs;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Completed evaluation
/// </summary>
public sealed class Run
{
    /// <summary>
    /// Identifier
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Username of the evaluated employee
    /// </summary>
    public required string RunFor { get; init; }

    /// <summary>
    /// Final evaluation score
    /// </summary>
    public required decimal ScoreResult { get; init; }

    /// <summary>
    /// Timestamp of the last change to the questionnaire
    /// </summary>
    public DateTime? LastSavedAt { get; init; }

    /// <summary>
    /// Username of the employee who modified the questionnaire
    /// </summary>
    public string? LastSavedBy { get; init; }

    /// <summary>
    /// Timestamp of the first questionnaire save
    /// </summary>
    public DateTime? FirstSavedAt { get; init; }

    /// <summary>
    /// Username of the employee who performed the first evaluation
    /// </summary>
    public string? FirstSavedBy { get; init; }

    /// <summary>
    /// Timestamp when the evaluation results were published
    /// </summary>
    public DateTime? PublishedAt { get; init; }

    /// <summary>
    /// Username of the employee who published the evaluation
    /// </summary>
    public string? PublishedBy { get; init; }

    /// <summary>
    /// Creation timestamp of the evaluation object (technical creation without evaluation)
    /// </summary>
    public required DateTime CreatedAt { get; init; }

    /// <summary>
    /// Username of the employee who created the evaluation object (technical creation without evaluation)
    /// </summary>
    public required string CreatedBy { get; init; }

    /// <summary>
    /// Initial context of the evaluation form
    /// </summary>
    public required IReadOnlyDictionary<string, string> Context { get; init; }

    /// <summary>
    /// Evaluation form identifier
    /// </summary>
    public long EvaluationFormId { get; init; }

    /// <summary>
    /// Comment for the evaluation form
    /// </summary>
    public string? EvaluationFormComment { get; init; }

    /// <summary>
    /// List of results per criterion
    /// </summary>
    public required IReadOnlyList<RunCriterionResult> RunCriterionResults { get; init; }

    /// <summary>
    /// Timestamp when the evaluation was viewed
    /// </summary>
    public DateTime? ViewedAt { get; init; }

    /// <summary>
    /// Agreement status with the evaluation
    /// </summary>
    public RunAgreementStatus? AgreementStatus { get; init; }

    /// <summary>
    /// Timestamp of the agreement status change
    /// </summary>
    public DateTime? AgreementAt { get; init; }
}
