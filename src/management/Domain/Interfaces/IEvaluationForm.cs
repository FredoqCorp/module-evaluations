using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces;

/// <summary>
/// Contract for the evaluation form aggregate root.
/// </summary>
public interface IEvaluationForm
{
    /// <summary>
    /// Returns the unique identifier.
    /// </summary>
    Identifiers.Uuid Id();

    /// <summary>
    /// Returns the human-facing metadata.
    /// </summary>
    IFormMeta Meta();

    /// <summary>
    /// Returns the lifecycle value object.
    /// </summary>
    IFormLifecycle Lifecycle();

    /// <summary>
    /// Returns the calculation rule kind.
    /// </summary>
    FormCalculationKind Rule();

    /// <summary>
    /// Returns the ordered groups of criteria.
    /// </summary>
    IImmutableList<FormGroup> Groups();

    /// <summary>
    /// Returns the criteria outside of any group.
    /// </summary>
    IImmutableList<FormCriterion> Criteria();
}
