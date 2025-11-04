namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;

/// <summary>
/// Provides access to an order index value used within ordered collections.
/// </summary>
public interface IOrderIndex
{
    /// <summary>
    /// Reads the order index as an integer.
    /// </summary>
    /// <returns>Order index value.</returns>
    int Value();
}
