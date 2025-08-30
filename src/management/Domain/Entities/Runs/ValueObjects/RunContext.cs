namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run context: arbitrary key/value pairs used by automatic criteria.
/// </summary>
public sealed record RunContext
{
    /// <summary>
    /// Context values. Keys and values are strings.
    /// </summary>
    public required IReadOnlyDictionary<string, string> Items { get; init; }
}
