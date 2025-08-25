namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Value object for a criterion title.
/// </summary>
public sealed record CriterionTitle
{
    /// <summary>
    /// Raw string value of the title.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Returns the raw title string.
    /// </summary>
    public override string ToString() => Value;
}
