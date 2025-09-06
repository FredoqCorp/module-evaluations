namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for evaluation form aggregates.
/// </summary>
public readonly record struct EvaluationFormId(Guid Value)
{
    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Value.ToString();
}

