using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Runs.ValueObjects;

/// <summary>
/// Immutable snapshot of a form captured at run launch time for stable addressing and reproducible calculation.
/// </summary>
public sealed record RunFormSnapshot : IRunFormSnapshot
{
    private readonly IFormMeta _meta;
    private readonly IImmutableList<IRunFormGroup> _groups;
    private readonly IImmutableList<IRunFormCriterion> _criteria;
    private readonly IId _formId;
    private readonly ICalculationPolicy _policy;

    /// <summary>
    /// Creates a form snapshot with meta, explicit runtime policy, ordered groups and root-level criteria.
    /// </summary>
    public RunFormSnapshot(IId formId, IFormMeta meta, ICalculationPolicy policy, IImmutableList<IRunFormGroup> groups, IImmutableList<IRunFormCriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(formId);
        ArgumentNullException.ThrowIfNull(meta);
        ArgumentNullException.ThrowIfNull(policy);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);
        _meta = meta;
        _groups = groups;
        _criteria = criteria;
        _formId = formId;
        _policy = policy;
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
    /// Returns the bound runtime calculation policy captured at launch time.
    /// </summary>
    public ICalculationPolicy Policy() => _policy;

    /// <summary>
    /// Returns the ordered groups of criteria captured at launch time.
    /// </summary>
    public IImmutableList<IRunFormGroup> Groups() => _groups;

    /// <summary>
    /// Returns the root-level criteria captured at launch time.
    /// </summary>
    public IImmutableList<IRunFormCriterion> Criteria() => _criteria;

}
