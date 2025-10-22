using System.Threading;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Behavioral contract for an unweighted groups collection used by average scoring.
/// </summary>
public interface IAverageGroups : IGroups
{
    /// <summary>
    /// Adds an average group directly under a form.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added average group.</returns>
    Task<IAverageGroup> Add(GroupProfile profile, FormId formId, OrderIndex orderIndex, CancellationToken ct = default);

    /// <summary>
    /// Adds an average group under another group.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="parentId">Identifier of the parent group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added average group.</returns>
    Task<IAverageGroup> Add(GroupProfile profile, GroupId parentId, OrderIndex orderIndex, CancellationToken ct = default);
}
