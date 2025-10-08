using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;

/// <summary>
/// Immutable root group for average scoring that prevents mixing with weighted members.
/// </summary>
public sealed class AverageRootGroup : IFormRootGroup, IAverageGroup
{
    private readonly IAverageCriteria _criteria;
    private readonly IAverageGroups _groups;

    /// <summary>
    /// Initializes the average root group with unweighted members.
    /// </summary>
    /// <param name="criteria">Unweighted criteria placed at the root.</param>
    /// <param name="groups">Unweighted groups placed at the root.</param>
    public AverageRootGroup(IAverageCriteria criteria, IAverageGroups groups)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(groups);

        _criteria = criteria;
        _groups = groups;
    }

    /// <summary>
    /// Calculates the contribution produced by root-level unweighted members.
    /// </summary>
    /// <returns>Contribution represented by the root group.</returns>
    public IRatingContribution Contribution()
    {
        IRatingContribution total = new RatingContribution(decimal.Zero, 0);

        total = total.Join(_criteria.Contribution());
        total = total.Join(_groups.Contribution());

        return total;
    }

    /// <summary>
    /// Validates the internal consistency of the average root group.
    /// </summary>
    public void Validate()
    {
        _criteria.Validate();
        _groups.Validate();
    }
}
