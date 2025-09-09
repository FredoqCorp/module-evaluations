namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Supported form audit action kinds.
/// </summary>
public enum FormAuditKind
{
    /// <summary>
    /// Creation of the form as a one time action.
    /// </summary>
    Created = 0,
    /// <summary>
    /// Edit of the form content before publish or archive.
    /// </summary>
    Edited = 1,
    /// <summary>
    /// Publication of the form content as a one time action.
    /// </summary>
    Published = 2,
    /// <summary>
    /// Archival of the form content as a one time action.
    /// </summary>
    Archived = 3
}
