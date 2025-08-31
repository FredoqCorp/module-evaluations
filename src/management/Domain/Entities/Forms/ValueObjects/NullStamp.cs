namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Null Object implementation of IStamp representing absence of a stamp.
/// </summary>
public sealed record NullStamp : IStamp
{
    /// <summary>
    /// Initializes a new null stamp representing absence of a real stamp.
    /// </summary>
    public NullStamp()
    {
    }

    /// <summary>
    /// Returns an empty user identifier string.
    /// </summary>
    public string UserId() => string.Empty;

    /// <summary>
    /// Returns a minimal timestamp value.
    /// </summary>
    public DateTime At() => DateTime.MinValue;
}
