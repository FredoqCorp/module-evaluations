namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

/// <summary>
/// Period of validity as an immutable value object. Start is required; End is optional.
/// </summary>
public sealed record Period
{
    private readonly DateTime _start;
    private readonly DateTime? _end;

    /// <summary>
    /// Creates a period with inclusive start and optional inclusive end.
    /// </summary>
    public Period(DateTime start, DateTime? end)
    {
        _start = start;
        _end = end;
    }

    /// <summary>
    /// Returns the inclusive start of the period.
    /// </summary>
    public DateTime Start()
    {
        return _start;
    }

    /// <summary>
    /// Returns the inclusive end of the period when present.
    /// </summary>
    public DateTime? End()
    {
        return _end;
    }
}
