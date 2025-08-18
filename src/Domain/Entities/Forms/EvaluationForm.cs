using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Aggregate root describing a form. Stores metadata, lifecycle and the calculation rule kind only.
/// Execution of scoring occurs in a separate "form run" domain object.
/// </summary>
public sealed class EvaluationForm
{
    private readonly List<FormCriterion> _criteria = [];
    private readonly List<FormGroup> _groups = [];

    /// <summary>
    /// Unique identifier.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Human-facing meta: name, description and tags.
    /// </summary>
    public required FormMeta Meta { get; init; }

    /// <summary>
    /// Lifecycle: status, validity period and audit trail.
    /// </summary>
    public required FormLifecycle Lifecycle { get; init; }

    /// <summary>
    /// Selected calculation rule
    /// </summary>
    public FormCalculationKind Calculation { get; init; } = FormCalculationKind.ArithmeticMean;

    /// <summary>
    /// Ordered groups of criteria.
    /// </summary>
    public IReadOnlyList<FormGroup> Groups => _groups;

    /// <summary>
    /// Criteria outside of any group
    /// </summary>
    public IReadOnlyList<FormCriterion> Criteria => _criteria;


    /// <summary>
    /// Adds the specified groups to the form and attaches them to this form instance.
    /// </summary>
    /// <param name="groups">The groups to add to the form.</param>
    /// <exception cref="ArgumentNullException">Thrown when the groups argument is null.</exception>
    public void AddGroups(IEnumerable<FormGroup> groups)
    {
        ArgumentNullException.ThrowIfNull(groups);
        foreach (var group in groups)
        {
            group.AttachToForm(this);
            _groups.Add(group);
        }
    }

    /// <summary>
    /// Adds the specified criteria to the form and attaches them to this form instance.
    /// </summary>
    /// <param name="criteria">The criteria to add to the form.</param>
    /// <exception cref="ArgumentNullException">Thrown when the criteria argument is null.</exception>
    public void AddCriteria(IEnumerable<FormCriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        _criteria.AddRange(criteria);
    }
}
