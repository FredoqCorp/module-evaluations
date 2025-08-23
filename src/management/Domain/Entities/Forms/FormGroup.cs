using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// A group of criteria within a form. Carries a title, order, optional weight and ordered criteria references.
/// </summary>
public sealed class FormGroup
{
    private readonly List<FormCriterion> _criteria = [];
    private readonly List<FormGroup> _groups = [];


    /// <summary>
    /// Technical identifier within the aggregate.
    /// </summary>
    public long Id { get; init; }

    /// <summary>
    /// Human-friendly title of the group.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// Display order of the group in the form.
    /// </summary>
    public required OrderIndex Order { get; init; }

    /// <summary>
    /// Optional weight of the group when WeightedMean is used.
    /// </summary>
    public Weight? Weight { get; init; }

    /// <summary>
    /// Criteria inside the group, ordered.
    /// </summary>
    public IReadOnlyList<FormCriterion> Criteria => _criteria;

    /// <summary>
    /// Nested groups inside this group
    /// </summary>
    public IReadOnlyList<FormGroup> Groups => _groups;

    /// <summary>
    /// The form to which this group belongs.
    /// </summary>
    public EvaluationForm Form { get; private set; } = null!;

    /// <summary>
    /// The identifier of the form to which this group belongs.
    /// </summary>
    public long FormId { get; init; }

    /// <summary>
    /// The parent group of this group, if any.
    /// </summary>
    public FormGroup? Parent { get; private set; }

    /// <summary>
    /// The identifier of the parent group, if any.
    /// </summary>
    public long? ParentId { get; init; }

    /// <summary>
    /// Adds a child group to this group and attaches it to the form if already set.
    /// </summary>
    /// <param name="child">The child group to add.</param>
    public void AddChilds(IEnumerable<FormGroup> child)
    {
        ArgumentNullException.ThrowIfNull(child);
        foreach (var c in child)
        {
            c.Parent = this;
            if (Form is not null)
            {
                c.AttachToForm(Form);
            }

            _groups.Add(c);
        }
    }

    /// <summary>
    /// Adds criteria to this group.
    /// </summary>
    /// <param name="criteria">The criteria to add to the group.</param>
    public void AddCriteria(IEnumerable<FormCriterion> criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        _criteria.AddRange(criteria);
    }

    internal void AttachToForm(EvaluationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
        Form = form;
        foreach (var g in _groups)
        {
            g.AttachToForm(form);
        }
    }
}
