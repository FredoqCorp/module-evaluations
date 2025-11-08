using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// Display order index within a collection as an immutable value object.
/// </summary>
public sealed record OrderIndex : IOrderIndex
{
    private readonly int _position;

    /// <summary>
    /// Creates a display order index with a raw integer value.
    /// </summary>
    public OrderIndex(int value)
    {
        _position = value;
    }

    /// <summary>
    /// Reads the order index value.
    /// </summary>
    /// <returns>Order index value.</returns>
    public int Value() => _position;
}
