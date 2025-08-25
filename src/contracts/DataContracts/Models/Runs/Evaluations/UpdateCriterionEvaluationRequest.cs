using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Evaluations;

/// <summary>
/// Represents a request to update the evaluation of a single criterion within an evaluation run
/// </summary>
/// <param name="CriterionId">Unique identifier of the criterion to update</param>
/// <param name="Score">Score on a 1–5 scale where 1 is the lowest and 5 is the highest</param>
/// <param name="Skipped">True if the criterion was intentionally skipped, otherwise false</param>
/// <param name="Comment">Optional human note with additional context</param>
public sealed record UpdateCriterionEvaluationRequest([Required] long CriterionId,
    [Required][Range(1, 5)] ushort Score,
    [Required] bool Skipped,
    string? Comment);