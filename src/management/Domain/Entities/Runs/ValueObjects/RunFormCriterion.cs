using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Run-level decorated form criterion that wraps an original form criterion and adds a run-local key.
/// </summary>
public sealed record RunFormCriterion : IRunFormCriterion
{
    private readonly IId _id;
    private readonly IFormCriterion _inner;

    /// <summary>
    /// Creates a decorated form criterion with a run-local key and an original criterion instance.
    /// </summary>
    public RunFormCriterion(IId id, IFormCriterion inner)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(inner);
        _id = id;
        _inner = inner;
    }

    /// <summary>
    /// Returns the stable identifier of this criterion within the snapshot.
    /// </summary>
    public IId Id() => _id;

    /// <summary>
    /// Returns the domain criterion value object by delegating to the wrapped criterion.
    /// </summary>
    public ICriterion Criterion() => _inner.Criterion();

    /// <summary>
    /// Returns the display order index by delegating to the wrapped criterion.
    /// </summary>
    public IOrderIndex Order() => _inner.Order();

    
}
