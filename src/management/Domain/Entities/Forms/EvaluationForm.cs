using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Aggregate root describing a form. Stores metadata, lifecycle and the calculation rule kind only.
/// Execution of scoring occurs in a separate "form run" domain object.
/// </summary>
public sealed class EvaluationForm : IEvaluationForm
{
    private readonly IId _id;
    private readonly IFormMeta _meta;
    private readonly IFormLifecycle _lifecycle;
    private readonly IImmutableList<IFormGroup> _groups;
    private readonly IImmutableList<IFormCriterion> _criteria;
    private readonly ICalculationPolicyDefinition _definition;

    /// <summary>
    /// Creates an evaluation form aggregate with identifier, meta, lifecycle, calculation definition, groups and criteria.
    /// </summary>
    public EvaluationForm(
        Identifiers.EvaluationFormId id,
        IFormMeta meta,
        IFormLifecycle lifecycle,
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
    /// Returns the lifecycle value object of this evaluation form aggregate.
    /// </summary>
    public IFormLifecycle Lifecycle() => _lifecycle;

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

        var groups = BuildGroups(_groups);
        var criteria = BuildCriteria(_criteria);
        var policy = _definition.Policy();
        return new RunFormSnapshot(_id, _meta, policy, groups, criteria);
    }

    private static ImmutableList<IRunFormGroup> BuildGroups(IImmutableList<IFormGroup> source)
    {
        var res = ImmutableList.CreateBuilder<IRunFormGroup>();
        foreach (var g in source)
        {
            var childrenGroups = BuildGroups(g.Groups());
            var childrenCriteria = BuildCriteria(g.Criteria());
            var rg = new RunFormGroup(g.Id(), g.Title(), g.Order(), childrenCriteria, childrenGroups);
            res.Add(rg);
        }
        return res.ToImmutable();
    }

    private static ImmutableList<IRunFormCriterion> BuildCriteria(IImmutableList<IFormCriterion> source)
    {
        var res = ImmutableList.CreateBuilder<IRunFormCriterion>();
        foreach (var c in source)
        {
            var rc = new RunFormCriterion(c.Id(), c);
            res.Add(rc);
        }
        return res.ToImmutable();
    }
}
