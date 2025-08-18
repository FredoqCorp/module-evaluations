namespace CascVel.Module.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Run metadata: who is evaluated, which form is used, and an optional supervisor comment.
/// </summary>
public sealed class RunMeta
{
    /// <summary>
    /// Reference to the form used by the run.
    /// </summary>
    public required RunFormRef Form { get; init; }

    /// <summary>
    /// Identifier of the evaluated subject (operator).
    /// </summary>
    public required string RunFor { get; init; }

    /// <summary>
    /// Supervisor comment for the entire form run.
    /// </summary>
    public string? SupervisorComment { get; init; }
}
