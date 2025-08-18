namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Display order index within a collection. Must be non-negative and unique in scope.
/// </summary>
public sealed record OrderIndex
{
    /// <summary>
    /// Zero-based order value.
    /// </summary>
    public required int Value { get; init; }
}
