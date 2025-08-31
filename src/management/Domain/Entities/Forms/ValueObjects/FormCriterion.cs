namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Criterion positioned within a form or a group, with order and optional weight.
/// </summary>
public sealed record FormCriterion : IFormCriterion
{
    private readonly Criterion _criterion;
    private readonly IOrderIndex _order;
    private readonly IWeight? _weight;

    /// <summary>
    /// Creates a positioned criterion with order and optional weight.
    /// </summary>
    public FormCriterion(Criterion criterion, IOrderIndex order, IWeight? weight)
    {
        ArgumentNullException.ThrowIfNull(criterion);
        ArgumentNullException.ThrowIfNull(order);

        _criterion = criterion;
        _order = order;
        _weight = weight;
    }

    /// <summary>
    /// Returns the domain criterion value object.
    /// </summary>
    public Criterion Criterion()
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
    /// Returns the optional weight when used.
    /// </summary>
    public IWeight? Weight()
    {
        return _weight;
    }
}
