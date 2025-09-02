using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Weighted definition of a group matching form structure.
/// </summary>
public sealed record WeightedGroupDefinition
{
    private readonly Weight _weight;
    private readonly IImmutableList<WeightedCriterionDefinition> _criteria;
    private readonly IImmutableList<WeightedGroupDefinition> _groups;

    /// <summary>
    /// Creates a weighted group definition with its weight and children.
    /// </summary>
    public WeightedGroupDefinition(Weight weight, IImmutableList<WeightedCriterionDefinition> criteria, IImmutableList<WeightedGroupDefinition> groups)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);
        _weight = weight;
        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Returns the weight for this group.
    /// </summary>
    public Weight Weight() => _weight;

    /// <summary>
    /// Returns weighted criteria children.
    /// </summary>
    public IImmutableList<WeightedCriterionDefinition> Criteria() => _criteria;

    /// <summary>
    /// Returns weighted nested groups.
    /// </summary>
    public IImmutableList<WeightedGroupDefinition> Groups() => _groups;
}
