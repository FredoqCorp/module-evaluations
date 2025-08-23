using System.Text.Json.Serialization;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Represents a group of form fields with a title, display order, and optional weight.
/// Usage example:
/// </summary>
/// <param name="Title">The title of the form group.</param>
/// <param name="Order">The display order of the form group.</param>
/// <param name="Weight">The optional weight of the form group.</param>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(UpdateFormGroupRequest), typeDiscriminator: nameof(UpdateFormGroupRequest))]
[JsonDerivedType(typeof(CreateFormGroupRequest), typeDiscriminator: nameof(CreateFormGroupRequest))]
public abstract record FormGroup(string Title, ushort Order, decimal? Weight);
