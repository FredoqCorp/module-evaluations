using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

/// <summary>
/// Criterion positioned within a form or a group, with order.
/// </summary>
public sealed record FormCriterion : IFormCriterion
{
    private readonly FormCriterionId _id;
    private readonly ICriterion _criterion;
    private readonly OrderIndex _order;

    /// <summary>
    /// Creates a positioned criterion with order and weight.
    /// </summary>
    public FormCriterion(FormCriterionId id, ICriterion criterion, OrderIndex order)
    {
        ArgumentNullException.ThrowIfNull(criterion);

        _id = id;
        _criterion = criterion;
        _order = order;
    }

    /// <summary>
    /// Returns the domain criterion value object.
    /// </summary>
    public ICriterion Criterion()
    {
        return _criterion;
    }

    /// <summary>
    /// Returns the display order index.
    /// </summary>
    public OrderIndex Order()
    {
        return _order;
    }

    /// <summary>
    /// Returns the stable identifier of this positioned criterion.
    /// </summary>
    public FormCriterionId Id()
    {
        return _id;
    }

    
}
