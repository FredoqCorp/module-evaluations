using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Runs;

/// <summary>
/// Immutable snapshot of a form captured at run launch time for stable addressing and reproducible calculation.
/// </summary>
public sealed record RunFormSnapshot : IRunFormSnapshot
{
    private readonly FormMeta _meta;
    private readonly FormGroupList _groups;
    private readonly FormCriteriaList _criteria;
    private readonly EvaluationFormId _formId;
    private readonly ICalculationPolicy _policy;

    /// <summary>
    /// Creates a form snapshot with meta, explicit runtime policy, ordered groups and root-level criteria.
    /// </summary>
    public RunFormSnapshot(EvaluationFormId formId, FormMeta meta, ICalculationPolicy policy, FormGroupList groups, FormCriteriaList criteria)
    {
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
    public EvaluationFormId FormId() => _formId;

    /// <summary>
    /// Returns human-facing metadata captured at launch time.
    /// </summary>
    public FormMeta Meta() => _meta;

    /// <summary>
    /// Returns the bound runtime calculation policy captured at launch time.
    /// </summary>
    public ICalculationPolicy Policy() => _policy;

    /// <summary>
    /// Returns the ordered groups of criteria captured at launch time.
    /// </summary>
    public FormGroupList Groups() => _groups;

    /// <summary>
    /// Returns the root-level criteria captured at launch time.
    /// </summary>
    public FormCriteriaList Criteria() => _criteria;

}
