using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Update;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a request to create a criterion view for a form, including the criterion details, its weight, and order.
/// </summary>
public sealed record UpdateFormCriterionViewRequest([Required] UpdateCriterionRequest Criterion, [Required] decimal? Weight, [Required] ushort Order);
