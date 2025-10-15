namespace CascVel.Modules.Evaluations.Management.Domain.Enums;

/// <summary>
/// Defines the roles available within the evaluations module.
/// </summary>
public enum ModuleRole
{
    /// <summary>
    /// Form designer who creates, edits, and manages evaluation forms.
    /// </summary>
    FormDesigner,

    /// <summary>
    /// Contact center operator who is being evaluated.
    /// </summary>
    Operator,

    /// <summary>
    /// Supervisor who evaluates operator performance.
    /// </summary>
    Supervisor
}
