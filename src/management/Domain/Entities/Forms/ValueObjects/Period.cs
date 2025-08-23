namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Period of validity. Start is required; End is optional (open-ended period when null).
/// </summary>
public sealed record Period
{
    /// <summary>
    /// Inclusive start of the period.
    /// </summary>
    public required DateTime Start { get; init; }

    /// <summary>
    /// Inclusive end of the period, if any. When null, the period is open-ended.
    /// </summary>
    public DateTime? End { get; init; }
}
