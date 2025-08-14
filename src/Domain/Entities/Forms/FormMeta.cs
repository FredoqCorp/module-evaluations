using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Human-facing meta information.
/// </summary>
public sealed class FormMeta
{
    /// <summary>
    /// Form name/title.
    /// </summary>
    public required FormName Name { get; init; }

    /// <summary>
    /// Optional description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Case-insensitive tags. Use an empty list when none.
    /// </summary>
    public required IReadOnlyList<string> Tags { get; init; }

    /// <summary>
    /// Globally-unique code. Editable only in Draft.
    /// </summary>
    public required FormCode Code { get; init; }
}
