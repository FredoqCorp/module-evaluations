using CascVel.Module.Evaluations.Management.Domain.Enums.Criteria;
using CascVel.Module.Evaluations.Management.Domain.ValueObjects;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;

/// <summary>
/// Represents an automatic parameter used in evaluation criteria.
/// </summary>
public sealed class AutomaticParameter
{
    /// <summary>
    /// Gets the unique identifier for the automatic parameter.
    /// </summary>
    public long Id { get; init; }
    /// <summary>
    /// Gets the caption or display name of the automatic parameter.
    /// </summary>
    public required string Caption { get; init; }
    /// <summary>
    /// Gets the condition type associated with the automatic parameter.
    /// </summary>
    public required AutomaticCriterionConditionType ConditionType { get; init; }
    /// <summary>
    /// Gets the service code associated with the automatic parameter.
    /// </summary>
    public required ServiceCode ServiceCode { get; init; }
}
