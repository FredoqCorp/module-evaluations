using CascVel.Modules.Evaluations.Management.DataContracts.Models.AutomaticParameters;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Response;



/// <summary>
/// Represents the response for an automatic criterion in a form.
/// </summary>
/// <param name="Id">The unique identifier of the criterion.</param>
/// <param name="Title">The title of the criterion.</param>
/// <param name="Evaluations">The list of evaluations associated with the criterion.</param>
/// <param name="Parameter">The automatic parameter associated with the criterion.</param>
public sealed record AutomaticCriterionResponse(long Id, string Title, IReadOnlyList<AutomaticCriterionEvaluation> Evaluations, AutomaticParameterResponse Parameter) : CriterionResponse(Id, Title);
