namespace CascVel.Module.Evaluations.Management.Domain.Entities.Runs;

/// <summary>
/// Evidence for an automatic criterion: which parameter and value led to the option selection.
/// </summary>
public sealed class RunAutoEvidence
{
    /// <summary>
    /// Parameter key (matches Automation.Source.ParameterKey in the criterion).
    /// </summary>
    public required string ParameterKey { get; init; }

    /// <summary>
    /// Metric value used during option selection.
    /// </summary>
    public required decimal Value { get; init; }
}
