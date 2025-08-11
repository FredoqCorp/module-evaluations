namespace CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria;

/// <summary>
/// Possible options for evaluation criteria.
/// </summary>
public sealed record EvaluationOption
{
    /// <summary>
    /// Gets the score associated with this evaluation option.
    /// </summary>
    public required ushort Score { get; init; }

    /// <summary>
    /// Gets the caption or label for this evaluation option.
    /// </summary>
    public string? Caption { get; init; }

    /// <summary>
    /// Gets the annotation or additional notes for this evaluation option.
    /// </summary>
    public string? Annotation { get; init; }
    /// <summary>
    /// Gets the condition value associated with this evaluation option.
    /// </summary>
    public decimal? ConditionValue { get; init; }
}
