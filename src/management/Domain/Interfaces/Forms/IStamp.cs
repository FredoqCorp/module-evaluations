namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

/// <summary>
/// Contract for an actor and time stamp.
/// </summary>
public interface IStamp
{
    /// <summary>
    /// Returns the user identifier.
    /// </summary>
    string UserId();

    /// <summary>
    /// Returns the time stamp.
    /// </summary>
    DateTime At();
}

