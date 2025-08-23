using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Create;


/// <summary>
/// Represents the base contract for creating a criterion in a form.
/// </summary>
/// <param name="Title">The title of the criterion, required and limited to 365 characters.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CreateDefaultCriterionRequest), typeDiscriminator: nameof(CreateDefaultCriterionRequest))]
[JsonDerivedType(typeof(CreateAutomaticCriterionRequest), typeDiscriminator: nameof(CreateAutomaticCriterionRequest))]
public abstract record CreateCriterionRequest([Required][MaxLength(365)] string Title);
