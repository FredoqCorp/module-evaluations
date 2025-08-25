using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Update;


/// <summary>
/// Represents an automatic criterion update for a form.
/// <param name="Id">The unique identifier of the criterion.</param>
/// <param name="Title">The title of the criterion.</param>
/// <param name="Evaluations">The list of evaluations associated with the criterion.</param>
/// <param name="ParameterId">The unique identifier of the parameter.</param>
/// </summary>
public sealed record UpdateAutomaticCriterionRequest(long Id, string Title, [Required] IReadOnlyList<AutomaticCriterionEvaluation> Evaluations, [Required] long ParameterId) : UpdateCriterionRequest(Id, Title);
