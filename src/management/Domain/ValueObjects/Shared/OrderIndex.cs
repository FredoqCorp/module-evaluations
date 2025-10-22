namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

/// <summary>
/// Display order index within a collection as an immutable value object.
/// </summary>
public readonly record struct OrderIndex
{

    /// <summary>
    /// Creates a display order index with a raw integer value.
    /// </summary>
    public OrderIndex(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);

        Value = value;
    }

    /// <summary>
    /// Underlying integer value representing the order index.
    /// </summary>
    public int Value { get; init; }
}
