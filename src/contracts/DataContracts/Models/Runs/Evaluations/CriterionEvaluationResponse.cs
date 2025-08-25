using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Evaluations;

/// <summary>
/// Base response for an evaluation of a single criterion within a run.
/// </summary>
/// <param name="CriterionId">The ID of the criterion being evaluated</param>
/// <param name="Score">Assigned score from 1 to 5</param>
/// <param name="Skipped">Whether the criterion was skipped by the evaluator</param>
/// <param name="Comment">Optional evaluator comment, up to 365 chars</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(DefaultCriterionEvaluationResponse), typeDiscriminator: nameof(DefaultCriterionEvaluationResponse))]
[JsonDerivedType(typeof(AutomaticCriterionEvaluationResponse), typeDiscriminator: nameof(AutomaticCriterionEvaluationResponse))]
public abstract record CriterionEvaluationResponse(
    long CriterionId,
    ushort Score,
    bool Skipped,
    string? Comment
);
