namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Globally-unique code of a form. Change is allowed only in Draft.
/// </summary>
public sealed record FormCode
{
    /// <summary>
    /// Raw string value of the code.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Returns the raw code.
    /// </summary>
    public override string ToString() => Value;
}
