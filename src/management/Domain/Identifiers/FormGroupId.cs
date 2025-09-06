namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// Strongly-typed identifier for form group value objects.
/// </summary>
public readonly record struct FormGroupId(Guid Value)
{
    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Value.ToString();
}

