using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Weighted definition for a single criterion.
/// </summary>
public sealed record WeightedCriterionDefinition
{
    private readonly Weight _weight;

    /// <summary>
    /// Creates a weighted criterion definition with the provided weight.
    /// </summary>
    public WeightedCriterionDefinition(Weight weight)
    {
        _weight = weight;
    }

    /// <summary>
    /// Returns the weight.
    /// </summary>
    public Weight Weight() => _weight;
}
