namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Filters;

/// <summary>
/// Represents a filter for querying run entities.
/// </summary>
public sealed class RunFilter
{
    /// <summary>
    /// Gets the identifier for which the run is intended.
    /// </summary>
    public string? RunFor { get; init; }

    /// <summary>
    /// Gets the minimum date and time for the last change of the run.
    /// </summary>
    public DateTime? RunChangeDateFrom { get; init; }


    /// <summary>
    /// Gets the maximum date and time for the last change of the run.
    /// </summary>
    public DateTime? RunChangeDateUntil { get; init; }


    /// <summary>
    /// Gets a value indicating whether to show only runs that have not been viewed.
    /// </summary>
    public bool? OnlyNotViewed { get; init; }

    /// <summary>
    /// Gets a value indicating whether to show only runs that have been published.
    /// </summary>
    public bool? OnlyPublished { get; init; }

    /// <summary>
    /// Gets the context parameters for the run.
    /// </summary>
    public IDictionary<string, string>? Context { get; init; }
}
