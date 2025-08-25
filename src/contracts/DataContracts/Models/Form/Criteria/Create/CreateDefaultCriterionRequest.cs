using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Create;

/// <summary>
/// Represents a request to create a default criterion with a title and a list of evaluations.
/// </summary>
public sealed record CreateDefaultCriterionRequest(string Title, [Required] IReadOnlyList<CriterionEvaluation> Evaluations) : CreateCriterionRequest(Title);
