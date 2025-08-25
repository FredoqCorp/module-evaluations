using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Runs;

/// <summary>
/// Represents a request to update a run with new evaluation criteria results, total score, and optional comment.
/// </summary>
/// <param name="Evaluations">The list of criterion evaluation requests to update.</param>
/// <param name="ScoreResult">The resulting score for the run.</param>
/// <param name="Comment">An optional comment for the run update.</param>
public sealed record UpdateRunRequest(
    [Required] IReadOnlyList<UpdateCriterionEvaluationRequest> Evaluations,
    [Required] decimal ScoreResult,
    string? Comment
);
