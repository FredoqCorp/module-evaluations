using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Criteria.Response;

/// <summary>
/// Represents a response for a criterion in a form, used as a base for specific criterion response types.
/// </summary>
/// <param name="Id">The unique identifier of the criterion response.</param>
/// <param name="Title">The title of the criterion response.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(DefaultCriterionResponse), typeDiscriminator: nameof(DefaultCriterionResponse))]
[JsonDerivedType(typeof(AutomaticCriterionResponse), typeDiscriminator: nameof(AutomaticCriterionResponse))]
public abstract record CriterionResponse(long Id, string Title);
