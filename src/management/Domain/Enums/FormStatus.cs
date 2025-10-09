namespace CascVel.Modules.Evaluations.Management.Domain.Enums;

/// <summary>
/// Represents the lifecycle state of an evaluation form: Draft, Published, or Archived.
/// </summary>
public enum FormStatus
{
    /// <summary>
    /// Form is being authored and may be edited freely.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Form is visible to evaluators and cannot be modified except archival.
    /// </summary>
    Published = 1,

    /// <summary>
    /// Form is read-only and removed from active use; no further transitions are allowed.
    /// </summary>
    Archived = 2
}
