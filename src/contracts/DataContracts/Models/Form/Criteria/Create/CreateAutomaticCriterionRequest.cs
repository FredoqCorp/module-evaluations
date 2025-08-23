using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Create;


///
/// <summary>
/// Represents a criterion that have an automatic parameter to calculate evaluation with a title, a list of evaluations, and a parameter identifier.
/// </summary>
/// <param name="Title">The title of the criterion.</param>
/// <param name="Evaluations">The list of evaluations associated with the criterion.</param>
/// <param name="ParameterId">The identifier of the automatic parameter for this criterion.</param>
public sealed record CreateAutomaticCriterionRequest(string Title, [Required] IReadOnlyList<AutomaticCriterionEvaluation> Evaluations, [Required] long ParameterId) : CreateCriterionRequest(Title);
