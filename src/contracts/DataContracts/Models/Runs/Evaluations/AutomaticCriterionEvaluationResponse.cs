namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Evaluations;

/// <summary>
/// Evaluation response for automatic criteria, including the condition value the score was based on.
/// </summary>
/// <param name="CriterionId">The ID of the criterion being evaluated</param>
/// <param name="Score">Assigned score from 1 to 5</param>
/// <param name="Skipped">Whether the criterion was skipped by the evaluator</param>
/// <param name="Comment">Optional evaluator comment, up to 365 chars</param>
/// <param name="ConditionValue">The automatic parameter value used to compute the score</param>
public sealed record AutomaticCriterionEvaluationResponse(
    long CriterionId,
    ushort Score,
    bool Skipped,
    string? Comment,
    decimal ConditionValue
) : CriterionEvaluationResponse(CriterionId, Score, Skipped, Comment);
