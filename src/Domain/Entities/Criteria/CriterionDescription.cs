namespace CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Value object for a criterion description.
/// </summary>
public sealed record CriterionDescription
{
    /// <summary>
    /// Raw string value of the description.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Returns the raw description string.
    /// </summary>
    public override string ToString() => Value;
}
