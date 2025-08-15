using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;

namespace CascVel.Module.Evaluations.Management.Domain.Entities.Forms;

/// <summary>
/// Design of the form: calculation rule and groups.
/// </summary>
public sealed class FormDesign
{
    /// <summary>
    /// Selected calculation rule
    /// </summary>
    public FormCalculationKind Calculation { get; init; } = FormCalculationKind.ArithmeticMean;

    /// <summary>
    /// Ordered groups of criteria.
    /// </summary>
    public required IReadOnlyList<FormGroup> Groups { get; init; } = [];

    /// <summary>
    /// Criteria outside of any group
    /// </summary>
    public required IReadOnlyList<FormCriterion> Criteria { get; init; } = [];
}
