namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Criterion positioned within a form or a group, with order and optional weight.
/// </summary>
public sealed record FormCriterion : IFormCriterion
{
    private readonly ICriterion _criterion;
    private readonly IOrderIndex _order;
    private readonly IWeight _weight;

    /// <summary>
    /// Creates a positioned criterion with order and weight.
    /// </summary>
    public FormCriterion(ICriterion criterion, IOrderIndex order, IWeight weight)
    {
        ArgumentNullException.ThrowIfNull(criterion);
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(weight);

        _criterion = criterion;
        _order = order;
        _weight = weight;
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
    /// Returns the weight when used.
    /// </summary>
    public IWeight Weight()
    {
        return _weight;
    }
}
