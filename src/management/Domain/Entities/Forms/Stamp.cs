namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Actor/time stamp.
/// </summary>
public sealed record Stamp
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public required string UserId { get; init; }

    /// <summary>
    /// Time of the action (UTC recommended).
    /// </summary>
    public required DateTime At { get; init; }
}
