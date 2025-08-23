using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

/// <summary>
/// Represents the evaluation of a single criterion in a form, including caption, score, and annotation.
/// </summary>
/// <param name="Caption">The caption or title of the criterion</param>
/// <param name="Score">The score assigned to the criterion, from 1 to 5</param>
/// <param name="Annotation">Optional annotation or comment for the evaluation</param>
public sealed record CriterionEvaluation(string? Caption, [Required][Range(1, 5)] ushort Score, [MaxLength(365)] string? Annotation);
