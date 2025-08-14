namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Non-negative percentage weight. For groups/criteria in the form.
/// </summary>
public sealed record Weight
{
    /// <summary>
    /// Value in percents, 0..100. Total must sum to 100% in context.
    /// </summary>
    public required decimal Percent { get; init; }
}
