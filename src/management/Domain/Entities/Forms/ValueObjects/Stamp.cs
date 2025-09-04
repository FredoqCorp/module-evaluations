using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;

/// <summary>
/// Actor and time stamp as an immutable value object.
/// </summary>
public sealed record Stamp : IStamp
{
    private readonly string _userId;
    private readonly DateTime _at;

    /// <summary>
    /// Creates a stamp with non-null user identifier and time.
    /// </summary>
    public Stamp(string userId, DateTime at)
    {
        ArgumentNullException.ThrowIfNull(userId);
        _userId = userId;
        _at = at;
    }

    /// <summary>
    /// Returns the user identifier.
    /// </summary>
    public string UserId()
    {
        return _userId;
    }

    /// <summary>
    /// Returns the time of the action.
    /// </summary>
    public DateTime At()
    {
        return _at;
    }
}
