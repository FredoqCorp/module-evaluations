using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Identifiers;

/// <summary>
/// UUIDv7-based identifier value object for domain entities.
/// </summary>
public sealed record Uuid : IId
{
    private readonly Guid _value;

    /// <summary>
    /// Creates an identifier from an existing UUID value.
    /// </summary>
    public Uuid(Guid value)
    {
        _value = value;
    }

    /// <summary>
    /// Creates a new identifier with a generated UUIDv7 value.
    /// </summary>
    public Uuid() : this(Guid.CreateVersion7())
    {
    }

    /// <summary>
    /// Canonical string representation.
    /// </summary>
    public string Text() => _value.ToString();

    /// <summary>
    /// Returns canonical string representation.
    /// </summary>
    public override string ToString() => Text();
}

