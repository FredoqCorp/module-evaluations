using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;

/// <summary>
/// Run context: arbitrary key/value pairs used by automatic criteria as an immutable value object.
/// </summary>
public sealed record RunContext : IRunContext
{
    private readonly IImmutableDictionary<string, string> _items;

    /// <summary>
    /// Creates a run context with immutable key/value pairs.
    /// </summary>
    public RunContext(IImmutableDictionary<string, string> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _items = items;
    }

    /// <summary>
    /// Returns the context items as an immutable dictionary.
    /// </summary>
    public IImmutableDictionary<string, string> Items() => _items;
}
