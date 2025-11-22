using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Groups;

/// <summary>
/// Unique identifier for a criterion group.
/// </summary>
public readonly record struct GroupId
{
    private readonly Guid _value;

    /// <summary>
    /// Creates a group identifier from a JSON element.
    /// </summary>
    /// <param name="node">JSON node containing the identifier field.</param>
    public GroupId(JsonElement node) : this(node.OptionalGuid("id").Reduce(Guid.CreateVersion7))
    {
    }

    /// <summary>
    /// Creates group identifier ensuring it is not empty.
    /// </summary>
    /// <param name="value">Non-empty GUID representing the group.</param>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public GroupId(Guid value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, Guid.Empty);
        _value = value;
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
    public Guid Value() => _value;
}
