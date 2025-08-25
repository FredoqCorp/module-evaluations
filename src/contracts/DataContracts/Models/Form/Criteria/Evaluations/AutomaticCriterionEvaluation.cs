using System.ComponentModel.DataAnnotations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;


/// <summary>
/// Represents the result of an automatic evaluation of a criterion in a form.
/// </summary>
/// <param name="Caption">The caption of the criterion.</param>
/// <param name="Score">The score assigned, from 1 to 5.</param>
/// <param name="Annotation">An optional annotation, up to 365 characters.</param>
/// <param name="ConditionValue">The value of the condition used for evaluation.</param>
public sealed record AutomaticCriterionEvaluation(string? Caption, [Required][Range(1, 5)] ushort Score, [MaxLength(365)] string? Annotation, [Required] float ConditionValue);
