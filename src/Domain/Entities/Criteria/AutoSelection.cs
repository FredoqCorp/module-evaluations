namespace CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Describes how a criterion's option is selected automatically at form fill time.
/// </summary>
public sealed class AutoSelection
{
    /// <summary>
    /// Source of the value used to determine the option.
    /// </summary>
    public required AutoSource Source { get; init; }

    /// <summary>
    /// Rule that maps the source value to an option's score.
    /// </summary>
    public required AutoRule Rule { get; init; }
}
