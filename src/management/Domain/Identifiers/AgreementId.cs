namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for agreement-related records.
/// </summary>
public readonly record struct AgreementId(Guid Value)
{
    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Value.ToString();
}

