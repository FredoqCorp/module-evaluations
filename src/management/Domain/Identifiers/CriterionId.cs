namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for a criterion stored as UUIDv7.
/// </summary>
public readonly record struct CriterionId
{
    /// <summary>
    /// Underlying UUID value stored as Guid with UUIDv7 semantics.
    /// </summary>
    public required Guid Value { get; init; }

    /// <summary>
    /// Returns canonical string representation of the identifier.
    /// </summary>
    public override string ToString() => Value.ToString();
}
