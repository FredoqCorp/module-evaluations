using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Display order index within a collection as an immutable value object.
/// </summary>
public sealed record OrderIndex : IOrderIndex
{
    private readonly int _value;

    /// <summary>
    /// Creates a display order index with a raw integer value.
    /// </summary>
    public OrderIndex(int value)
    {
        _value = value;
    }

    /// <summary>
    /// Returns the zero-based order value and fails fast when negative.
    /// </summary>
    public int Value()
    {
        if (_value < 0)
        {
            throw new InvalidDataException("Order index must not be negative");
        }

        return _value;
    }
}
