namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;

/// <summary>
/// Unique identifier for an evaluation criterion.
/// </summary>
public readonly record struct CriterionId
{
    /// <summary>
    /// Creates criterion identifier ensuring it is not empty.
    /// </summary>
    /// <param name="value">Non-empty GUID representing the criterion.</param>
    /// <exception cref="ArgumentException">Thrown when value is empty.</exception>
    public CriterionId(Guid value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, Guid.Empty);

        Value = value;
    }

    /// <summary>
    /// Creates a new criterion identifier with a newly generated GUID.
    /// </summary>
    public CriterionId() : this(Guid.CreateVersion7())
    {
    }

    /// <summary>
    /// Underlying GUID value representing the criterion identifier.
    /// </summary>
    public Guid Value { get; init; }
}
