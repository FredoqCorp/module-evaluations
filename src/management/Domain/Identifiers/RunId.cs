namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for a form run stored as UUIDv7.
/// </summary>
public readonly record struct RunId
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
