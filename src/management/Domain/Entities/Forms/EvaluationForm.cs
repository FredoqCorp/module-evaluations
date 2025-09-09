using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Aggregate root describing a form. Stores metadata, lifecycle and the calculation rule kind only.
/// Execution of scoring occurs in a separate "form run" domain object.
/// </summary>
public sealed class EvaluationForm : IEvaluationForm
{
    private readonly EvaluationFormId _id;
    private readonly FormMeta _meta;
    private readonly FormLifecycle _lifecycle;
    private readonly IImmutableList<IFormGroup> _groups;
    private readonly IImmutableList<IFormCriterion> _criteria;
    private readonly ICalculationPolicyDefinition _definition;

    /// <summary>
    /// Creates an evaluation form aggregate with identifier, meta, lifecycle, calculation definition, groups and criteria.
    /// </summary>
    public EvaluationForm(
        EvaluationFormId id,
        FormMeta meta,
        FormLifecycle lifecycle,
        IImmutableList<IFormGroup> groups,
        IImmutableList<IFormCriterion> criteria,
        ICalculationPolicyDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(lifecycle);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(definition);

        _id = id;
        _meta = meta;
        _lifecycle = lifecycle;
        _groups = groups;
        _criteria = criteria;
        _definition = definition;
    }

    /// <summary>
    /// Returns the ordered groups of criteria belonging to this evaluation form aggregate.
    /// </summary>
    public IImmutableList<IFormGroup> Groups() => _groups;

    /// <summary>
    /// Returns the criteria outside of any group belonging to this evaluation form aggregate.
    /// </summary>
    public IImmutableList<IFormCriterion> Criteria() => _criteria;


    /// <summary>
    /// Returns a run form snapshot for this evaluation form using stored calculation policy definition.
    /// </summary>
    public IRunFormSnapshot Snapshot()
    {
        _definition.Verify(this);

        var policy = _definition.Policy();
        return new RunFormSnapshot(_id, _meta, policy, _groups, _criteria);
    }

    /// <summary>
    /// Applies content changes to the form after validating audit rules and returns a new aggregate instance.
    /// </summary>
    public IEvaluationForm Edit(FormMeta meta, IImmutableList<IFormGroup> groups, IImmutableList<IFormCriterion> criteria, ICalculationPolicyDefinition definition, Stamp stamp)
    {
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);
        ArgumentNullException.ThrowIfNull(definition);

        var tail = _lifecycle.Tail;
        var nextTail = tail.Accept(FormAuditKind.Edited, stamp);

        var life = new FormLifecycle(_lifecycle.Validity, nextTail);
        return new EvaluationForm(_id, meta, life, groups, criteria, definition);
    }

    /// <summary>
    /// Publishes the form after validating audit rules and returns a new aggregate instance.
    /// </summary>
    public IEvaluationForm Publish(Stamp stamp)
    {
        var tail = _lifecycle.Tail;
        var nextTail = tail.Accept(FormAuditKind.Published, stamp);

        var life = new FormLifecycle(_lifecycle.Validity, nextTail);
        return new EvaluationForm(_id, _meta, life, _groups, _criteria, _definition);
    }

    /// <summary>
    /// Archives the form after validating audit rules and returns a new aggregate instance.
    /// </summary>
    public IEvaluationForm Archive(Stamp stamp)
    {
        var tail = _lifecycle.Tail;
        var nextTail = tail.Accept(FormAuditKind.Archived, stamp);

        var life = new FormLifecycle(_lifecycle.Validity, nextTail);
        return new EvaluationForm(_id, _meta, life, _groups, _criteria, _definition);
    }

    /// <inheritdoc/>
    public FormStatus Status() => _lifecycle.Tail.Kind() switch
    {
        FormAuditKind.Created => FormStatus.Draft,
        FormAuditKind.Edited => FormStatus.Draft,
        FormAuditKind.Published => FormStatus.Published,
        FormAuditKind.Archived => FormStatus.Archived,
        _ => FormStatus.Draft,
    };
}
