using System.Threading;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

/// <summary>
/// Behavioral contract for a groups collection that enforces weighted siblings.
/// </summary>
public interface IWeightedGroups : IGroups
{
    /// <summary>
    /// Adds a weighted group directly under a form.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted group.</returns>
    Task<IWeightedGroup> Add(GroupProfile profile, FormId formId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default);

    /// <summary>
    /// Adds a weighted group under another group.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="parentId">Identifier of the parent group.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted group.</returns>
    Task<IWeightedGroup> Add(GroupProfile profile, GroupId parentId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default);

    /// <summary>
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the groups expressed in basis points.</returns>
    IBasisPoints Weight();
}
