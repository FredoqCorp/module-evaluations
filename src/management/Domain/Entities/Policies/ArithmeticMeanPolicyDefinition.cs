using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Design-time definition of arithmetic mean policy without parameters.
/// </summary>
public sealed record ArithmeticMeanPolicyDefinition : ICalculationPolicyDefinition
{
    /// <summary>
    /// Returns stable policy code.
    /// </summary>
    public string Code() => "arithmetic-mean";

    /// <summary>
    /// Verifies compatibility of the definition with the form which is always true for arithmetic mean.
    /// </summary>
    public void Verify(IEvaluationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);
    }

    /// <summary>
    /// Binds the definition to the snapshot and returns a runtime policy.
    /// </summary>
    public ICalculationPolicy Policy() => new ArithmeticMeanPolicy();
}
