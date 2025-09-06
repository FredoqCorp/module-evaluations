namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for form criterion value objects.
/// </summary>
public readonly record struct FormCriterionId(Guid Value)
{
    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Value.ToString();
}

