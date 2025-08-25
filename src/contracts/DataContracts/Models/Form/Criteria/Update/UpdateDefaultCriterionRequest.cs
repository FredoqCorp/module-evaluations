using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Update;


/// <summary>
/// Represents a criterion for updating.
/// </summary>
/// <param name="Id">The unique identifier of the criterion.</param>
/// <param name="Title">The title of the criterion.</param>
/// <param name="Evaluations">The list of evaluations associated with the criterion.</param>
public sealed record UpdateDefaultCriterionRequest(long Id, string Title, [Required] IReadOnlyList<CriterionEvaluation> Evaluations) : UpdateCriterionRequest(Id, Title);