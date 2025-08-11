namespace CascVel.Module.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Represents the result of a criterion evaluation in a run.
/// </summary>
public sealed class RunCriterionResult
{
    /// <summary>
    /// Gets the unique identifier for the run criterion result.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Gets the comment associated with the run criterion result.
    /// </summary>
    public string? Comment { get; init; }

    /// <summary>
    /// Gets a value indicating whether the criterion was skipped in the run.
    /// </summary>
    public required bool IsSkipped { get; init; }

    /// <summary>
    /// Gets the automatic criterion value for the run, if available.
    /// </summary>
    public decimal? AutomaticCriterionValue { get; init; }

    /// <summary>
    /// Gets the score assigned to the run criterion result, if available.
    /// </summary>
    public ushort? Score { get; init; }

    /// <summary>
    /// Gets the identifier of the criterion associated with the run criterion result, if available.
    /// </summary>
    public long? CriterionId { get; init; }
}
