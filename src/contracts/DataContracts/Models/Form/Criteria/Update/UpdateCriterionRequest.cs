using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Update;


/// <summary>
/// Represents the base contract for creating a criterion in a form.
/// </summary>
/// <param name="Id">The unique identifier of the criterion.</param>
/// <param name="Title">The title of the criterion, required and limited to 365 characters.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(UpdateDefaultCriterionRequest), typeDiscriminator: nameof(UpdateDefaultCriterionRequest))]
[JsonDerivedType(typeof(UpdateAutomaticCriterionRequest), typeDiscriminator: nameof(UpdateAutomaticCriterionRequest))]
public abstract record UpdateCriterionRequest([Required] long Id, [Required][MaxLength(365)] string Title);
