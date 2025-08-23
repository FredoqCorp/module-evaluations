using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Create;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a request to create a criterion view for a form, including the criterion details, its weight, and order.
/// </summary>
public sealed record class CreateFormCriterionViewRequest([Required] CreateCriterionRequest Criterion, [Required] decimal? Weight, [Required] ushort Order);
