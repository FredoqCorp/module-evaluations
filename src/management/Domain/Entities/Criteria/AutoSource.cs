namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria;

/// <summary>
/// Describes an external or internal data source for automated selection.
/// </summary>
public sealed class AutoSource
{
    /// <summary>
    /// The name/key of the parameter to fetch from the service.
    /// </summary>
    public required string ParameterKey { get; init; }
}
