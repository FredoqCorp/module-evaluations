using System;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Models.Shared;

/// <summary>
/// Immutable decorator ensuring that order index values remain non-negative.
/// </summary>
public sealed class ValidOrderIndex : IOrderIndex
{
    private readonly IOrderIndex _original;

    /// <summary>
    /// Initializes the decorator with the original order index source.
    /// </summary>
    /// <param name="original">Order index source to validate.</param>
    public ValidOrderIndex(IOrderIndex original)
    {
        ArgumentNullException.ThrowIfNull(original);
        _original = original;
    }

    /// <inheritdoc />
    public int Value()
    {
        var value = _original.Value();
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
        return value;
    }
}
