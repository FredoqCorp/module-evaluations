namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects;

/// <summary>
/// Actor and time stamp as an immutable value object.
/// </summary>
public readonly record struct Stamp(string UserId, DateTime At);
