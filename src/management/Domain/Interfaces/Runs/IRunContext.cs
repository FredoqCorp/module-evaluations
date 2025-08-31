namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

using System.Collections.Immutable;

/// <summary>
/// Contract for run context key/value items.
/// </summary>
public interface IRunContext
{
    /// <summary>
    /// Returns the context items as an immutable dictionary.
    /// </summary>
    IImmutableDictionary<string, string> Items();
}

