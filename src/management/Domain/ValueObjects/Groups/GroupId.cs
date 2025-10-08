namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Unique identifier for a criterion group.
/// </summary>
public readonly record struct GroupId
{
    /// <summary>
    /// Creates group identifier ensuring it is not empty.
    /// </summary>
    /// <param name="value">Non-empty GUID representing the group.</param>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public GroupId(Guid value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, Guid.Empty);
        Value = value;
    }

    /// <summary>
    /// Creates a new group identifier with a newly generated GUID.
    /// </summary>
    public GroupId() : this(Guid.CreateVersion7())
    {
    }

    /// <summary>
    /// Underlying GUID value representing the group identifier.
    /// </summary>
    public Guid Value { get; init; }
}
