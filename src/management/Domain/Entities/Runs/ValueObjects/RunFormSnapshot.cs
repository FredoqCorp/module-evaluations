using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Immutable snapshot of a form captured at run launch time for stable addressing and reproducible calculation.
/// </summary>
public sealed record RunFormSnapshot : IRunFormSnapshot
{
    private readonly IFormMeta _meta;
    private readonly FormCalculationKind _rule;
    private readonly IImmutableList<IRunFormGroup> _groups;
    private readonly IImmutableList<IRunFormCriterion> _criteria;
    private readonly IId _formId;

    /// <summary>
    /// Creates a form snapshot with meta, rule, ordered groups and root-level criteria.
    /// </summary>
    public RunFormSnapshot(IId formId, IFormMeta meta, FormCalculationKind rule, IImmutableList<IRunFormGroup> groups, IImmutableList<IRunFormCriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(formId);
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);
        _meta = meta;
        _rule = rule;
        _groups = groups;
        _criteria = criteria;
        _formId = formId;
    }

    /// <summary>
    /// Returns the unique identifier of the form.
    /// </summary>
    public IId FormId() => _formId;

    /// <summary>
    /// Returns human-facing metadata captured at launch time.
    /// </summary>
    public IFormMeta Meta() => _meta;

    /// <summary>
    /// Returns the immutable form code captured at launch time derived from Meta.
    /// </summary>
    public string Code() => _meta.Code().Code();

    /// <summary>
    /// Returns the calculation rule kind captured at launch time.
    /// </summary>
    public FormCalculationKind Rule() => _rule;

    /// <summary>
    /// Returns the ordered groups of criteria captured at launch time.
    /// </summary>
    public IImmutableList<IRunFormGroup> Groups() => _groups;

    /// <summary>
    /// Returns the root-level criteria captured at launch time.
    /// </summary>
    public IImmutableList<IRunFormCriterion> Criteria() => _criteria;

}
