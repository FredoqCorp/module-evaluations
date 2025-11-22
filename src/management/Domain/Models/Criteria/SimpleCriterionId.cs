using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Extensions;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;

/// <summary>
/// Unique identifier for an evaluation criterion.
/// </summary>
public readonly record struct SimpleCriterionId : ICriterionId
{
    private readonly Guid _value;
    
    /// <summary>
    /// Creates a criterion identifier from a JSON element.
    /// </summary>
    /// <param name="node">JSON node containing the identifier field.</param>
    public SimpleCriterionId(JsonElement node) : this(node.OptionalGuid("id").Reduce(Guid.CreateVersion7))
    {
    }   

    /// <summary>
    /// Creates criterion identifier ensuring it is not empty.
    /// </summary>
    /// <param name="value">Non-empty GUID representing the criterion.</param>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public SimpleCriterionId(Guid value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, Guid.Empty);
        _value = value;
    }

    /// <summary>
    /// Creates a new criterion identifier with a newly generated GUID.
    /// </summary>
    public SimpleCriterionId() : this(Guid.CreateVersion7())
    {
    }

    /// <inheritdoc />
    public Guid Value() => _value;
}
