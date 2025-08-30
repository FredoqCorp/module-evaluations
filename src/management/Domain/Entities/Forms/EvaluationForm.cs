using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Identifiers;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Aggregate root describing a form. Stores metadata, lifecycle and the calculation rule kind only.
/// Execution of scoring occurs in a separate "form run" domain object.
/// </summary>
public sealed class EvaluationForm
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public FormId Id { get; init; }

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
    public required IReadOnlyList<FormGroup> Groups { get; init; }

    /// <summary>
    /// Criteria outside of any group
    /// </summary>
    public required IReadOnlyList<FormCriterion> Criteria { get; init; }
}
