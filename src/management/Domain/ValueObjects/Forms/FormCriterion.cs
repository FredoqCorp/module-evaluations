using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Criterion positioned within a form or a group, with order.
/// </summary>
public sealed record FormCriterion
{
    /// <summary>
    /// Creates a positioned criterion with order and weight.
    /// </summary>
    public FormCriterion(FormCriterionId id, Criterion criterion, OrderIndex order)
    {
        ArgumentNullException.ThrowIfNull(criterion);

        Id = id;
        Criterion = criterion;
        Order = order;
    }

    /// <summary>
    /// Returns the stable identifier of this positioned criterion.
    /// </summary>
    public FormCriterionId Id { get; }

    /// <summary>
    /// Returns the domain criterion value object.
    /// </summary>
    public Criterion Criterion { get; }

    /// <summary>
    /// Returns the display order index.
    /// </summary>
    public OrderIndex Order { get; }
}
