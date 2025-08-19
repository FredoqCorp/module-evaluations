namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Lifecycle status of an evaluation form.
/// </summary>
public enum FormStatus
{
    /// <summary>
    /// The form is in draft state and can be freely edited.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// The form is published; only metadata and validity period can be changed.
    /// </summary>
    Published = 1,

    /// <summary>
    /// The form is archived; final state.
    /// </summary>
    Archived = 2,
}
