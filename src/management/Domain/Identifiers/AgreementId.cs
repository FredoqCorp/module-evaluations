using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for agreement-related records.
/// </summary>
public readonly record struct AgreementId(Guid Value) : IId
{
    /// <summary>
    /// Canonical string representation.
    /// </summary>
    public string Text() => Value.ToString();

    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Text();
}

