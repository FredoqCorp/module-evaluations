namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for a display order index within a collection.
/// </summary>
public interface IOrderIndex
{
    /// <summary>
    /// Returns the zero-based order value.
    /// </summary>
    int Value();
}

