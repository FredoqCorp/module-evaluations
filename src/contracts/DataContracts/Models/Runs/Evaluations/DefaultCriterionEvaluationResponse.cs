namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Evaluations;

/// <summary>
/// Evaluation response for default/manual criteria.
/// </summary>
/// <param name="CriterionId">The ID of the criterion being evaluated</param>
/// <param name="Score">Assigned score from 1 to 5</param>
/// <param name="Skipped">Whether the criterion was skipped by the evaluator</param>
/// <param name="Comment">Optional evaluator comment, up to 365 chars</param>
public sealed record DefaultCriterionEvaluationResponse(
    long CriterionId,
    ushort Score,
    bool Skipped,
    string? Comment
) : CriterionEvaluationResponse(CriterionId, Score, Skipped, Comment);
