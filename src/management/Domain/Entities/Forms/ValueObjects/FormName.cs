namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Name/title of a form.
/// </summary>
public sealed record FormName
{
    /// <summary>
    /// Raw string value of the name.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Returns the raw name.
    /// </summary>
    public override string ToString() => Value;
}
