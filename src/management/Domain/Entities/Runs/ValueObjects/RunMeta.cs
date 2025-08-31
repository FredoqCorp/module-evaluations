using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run metadata: who is evaluated, which form is used, and an optional supervisor comment as an immutable value object.
/// </summary>
public sealed record RunMeta : IRunMeta
{
    private readonly IRunFormRef _form;
    private readonly string _runFor;
    private readonly string _supervisorComment;

    /// <summary>
    /// Creates a run metadata with form reference, subject identifier and optional supervisor comment.
    /// </summary>
    public RunMeta(IRunFormRef form, string runFor, string? supervisorComment)
    {
        ArgumentNullException.ThrowIfNull(form);
        ArgumentNullException.ThrowIfNull(runFor);
        _form = form;
        _runFor = runFor;
        _supervisorComment = supervisorComment ?? string.Empty;
    }

    /// <summary>
    /// Returns the reference to the form used by the run.
    /// </summary>
    public IRunFormRef Form() => _form;

    /// <summary>
    /// Returns the identifier of the evaluated subject.
    /// </summary>
    public string RunFor() => _runFor;

    /// <summary>
    /// Returns the supervisor comment as a non-null string.
    /// </summary>
    public string SupervisorComment() => _supervisorComment;
}
