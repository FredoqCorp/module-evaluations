namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Rule for translating a numeric KPI value into an option score.
/// Keep simple and implementation-agnostic for the domain model.
/// </summary>
public sealed class AutoRule
{
    /// <summary>
    /// Policy for interpreting per-option thresholds when auto-selecting an option.
    /// </summary>
    public required ThresholdPolicy ThresholdPolicy { get; init; }
}
