using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for evaluation form aggregates.
/// </summary>
public readonly record struct EvaluationFormId(Guid Value) : IId
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

