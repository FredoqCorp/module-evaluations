using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Reference to the evaluation form used for this run as an immutable value object.
/// </summary>
public sealed record RunFormRef : IRunFormRef
{
    private readonly Uuid _formId;
    private readonly string _formCode;

    /// <summary>
    /// Creates a reference to the evaluation form with identifier and code.
    /// </summary>
    public RunFormRef(Uuid formId, string formCode)
    {
        ArgumentNullException.ThrowIfNull(formCode);
        _formId = formId;
        _formCode = formCode;
    }

    /// <summary>
    /// Returns the form identifier.
    /// </summary>
    public Uuid FormId() => _formId;

    /// <summary>
    /// Returns the immutable form code captured at launch time.
    /// </summary>
    public string FormCode() => _formCode;
}
