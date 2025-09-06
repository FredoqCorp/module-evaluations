namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for form run aggregates.
/// </summary>
public readonly record struct RunId(Guid Value)
{
    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Value.ToString();
}

