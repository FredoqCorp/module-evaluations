using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;

/// <summary>
/// Run metadata: who is evaluated, which form is used, and an optional supervisor comment as an immutable value object.
/// </summary>
public sealed record RunMeta : IRunMeta
{
    private readonly IRunFormSnapshot _snapshot;
    private readonly string _runFor;
    private readonly string _supervisorComment;

    /// <summary>
    /// Creates a run metadata with form reference, snapshot, subject identifier and optional supervisor comment.
    /// </summary>
    public RunMeta(IRunFormSnapshot snapshot, string runFor, string? supervisorComment)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(runFor);
        _snapshot = snapshot;
        _runFor = runFor;
        _supervisorComment = supervisorComment ?? string.Empty;
    }

    /// <summary>
    /// Returns the snapshot of the form captured at launch time.
    /// </summary>
    public IRunFormSnapshot Snapshot() => _snapshot;

    /// <summary>
    /// Returns the identifier of the evaluated subject.
    /// </summary>
    public string RunFor() => _runFor;

    /// <summary>
    /// Returns the supervisor comment as a non-null string.
    /// </summary>
    public string SupervisorComment() => _supervisorComment;
}
