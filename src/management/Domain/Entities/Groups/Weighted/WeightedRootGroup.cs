using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;

/// <summary>
/// Immutable root group dedicated to weighted scoring that preserves sibling weight invariants.
/// </summary>
public sealed class WeightedRootGroup : IFormRootGroup
{
    private readonly IWeightedCriteria _criteria;
    private readonly IWeightedGroups _groups;

    /// <summary>
    /// Initializes the weighted root group with weighted members.
    /// </summary>
    /// <param name="criteria">Weighted criteria placed at the root.</param>
    /// <param name="groups">Weighted groups placed at the root.</param>
    public WeightedRootGroup(IWeightedCriteria criteria, IWeightedGroups groups)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Validates the internal consistency of the weighted root group.
    /// </summary>
    public void Validate()
    {
        _criteria.Validate();
        _groups.Validate();

        var criteria = decimal.ToInt32(_criteria.Weight().Apply(10000m));
        var groups = decimal.ToInt32(_groups.Weight().Apply(10000m));

        if (criteria == 0 && groups == 0)
        {
            throw new InvalidOperationException("Weighted root group must contain weighted members");
        }

        if (criteria > 0 && groups == 0 && criteria != 10000)
        {
            throw new InvalidOperationException("Root criteria weights must equal one hundred percent");
        }

        if (groups > 0 && criteria == 0 && groups != 10000)
        {
            throw new InvalidOperationException("Root group weights must equal one hundred percent");
        }

        if (criteria > 0 && groups > 0 && criteria + groups != 10000)
        {
            throw new InvalidOperationException("Root combined weights must equal one hundred percent");
        }
    }
}
