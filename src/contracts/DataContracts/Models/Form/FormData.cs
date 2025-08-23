using System.ComponentModel.DataAnnotations;
using CascVel.Modules.Evaluations.Management.DataContracts.Models.Form.Enums;

namespace CascVel.Modules.Evaluations.Management.DataContracts.Models.Form;

/// <summary>
/// Record that contains common fields for evaluation form
/// </summary>
/// <param name="Title">Form title</param>
/// <param name="Description">Form description</param>
/// <param name="Code">Form code</param>
/// <param name="ValidFrom">Form start availability date</param>
/// <param name="ValidUntil">Form end availability date</param>
/// <param name="CalculationRule">Rule to calculate final score</param>
/// <param name="FormKeywords">Keywords</param>
public sealed record FormData(
    [Required][MaxLength(365)] string Title,
    [MaxLength(365)] string? Description,
    [Required][MaxLength(72)][MinLength(3)] string Code,
    [DataType(DataType.DateTime)] DateTime? ValidFrom,
    [DataType(DataType.DateTime)] DateTime? ValidUntil,
    [Required][EnumDataType(typeof(CalculationRule))] CalculationRule CalculationRule,
    IReadOnlyList<string> FormKeywords
);
