using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Aggregate root describing a form. Stores metadata, lifecycle and the calculation rule kind only.
/// Execution of scoring occurs in a separate "form run" domain object.
/// </summary>
public sealed class EvaluationForm : IEvaluationForm
{
    private readonly Uuid _id;
    private readonly IFormMeta _meta;
    private readonly IFormLifecycle _lifecycle;
    private readonly FormCalculationKind _rule;
    private readonly IImmutableList<FormGroup> _groups;
    private readonly IImmutableList<FormCriterion> _criteria;

    /// <summary>
    /// Creates an evaluation form aggregate with identifier, meta, lifecycle, calculation rule, groups and criteria.
    /// </summary>
    public EvaluationForm(
        Uuid id,
        IFormMeta meta,
        IFormLifecycle lifecycle,
        FormCalculationKind rule,
        IImmutableList<FormGroup> groups,
        IImmutableList<FormCriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(lifecycle);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);

        _id = id;
        _meta = meta;
        _lifecycle = lifecycle;
        _rule = rule;
        _groups = groups;
        _criteria = criteria;
    }

    /// <summary>
    /// Returns the unique identifier of this evaluation form aggregate.
    /// </summary>
    public Identifiers.Uuid Id() => _id;

    /// <summary>
    /// Returns the human-facing metadata of this evaluation form aggregate.
    /// </summary>
    public IFormMeta Meta() => _meta;

    /// <summary>
    /// Returns the lifecycle value object of this evaluation form aggregate.
    /// </summary>
    public IFormLifecycle Lifecycle() => _lifecycle;

    /// <summary>
    /// Returns the calculation rule kind of this evaluation form aggregate.
    /// </summary>
    public FormCalculationKind Rule() => _rule;

    /// <summary>
    /// Returns the ordered groups of criteria belonging to this evaluation form aggregate.
    /// </summary>
    public IImmutableList<FormGroup> Groups() => _groups;

    /// <summary>
    /// Returns the criteria outside of any group belonging to this evaluation form aggregate.
    /// </summary>
    public IImmutableList<FormCriterion> Criteria() => _criteria;
}
