using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Evaluations;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Response;



/// <summary>
/// Represents the response model for a default criterion, including its evaluations.
/// </summary>
/// <param name="Id">The unique identifier of the criterion.</param>
/// <param name="Title">The title of the criterion.</param>
/// <param name="Evaluations">The list of evaluations associated with the criterion.</param>
public sealed record DefaultCriterionResponse(long Id, string Title, IReadOnlyList<CriterionEvaluation> Evaluations) : CriterionResponse(Id, Title);