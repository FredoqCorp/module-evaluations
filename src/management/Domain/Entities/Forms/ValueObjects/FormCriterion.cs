namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using Interfaces;

/// <summary>
/// Criterion positioned within a form or a group, with order and optional weight.
/// </summary>
public sealed record FormCriterion : IFormCriterion
{
    private readonly IId _id;
    private readonly ICriterion _criterion;
    private readonly IOrderIndex _order;

    /// <summary>
    /// Creates a positioned criterion with order and weight.
    /// </summary>
    public FormCriterion(IId id, ICriterion criterion, IOrderIndex order)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(criterion);
        ArgumentNullException.ThrowIfNull(order);

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
    public IOrderIndex Order()
    {
        return _order;
    }

    /// <summary>
    /// Returns the stable identifier of this positioned criterion.
    /// </summary>
    public IId Id()
    {
        return _id;
    }

    
}
