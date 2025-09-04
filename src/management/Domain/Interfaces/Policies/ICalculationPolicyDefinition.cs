using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;

/// <summary>
/// Design-time definition of a calculation policy attached to a form. Transforms into a runtime policy at launch.
/// </summary>
public interface ICalculationPolicyDefinition
{
    /// <summary>
    /// Returns stable code of the policy used for serialization and logs.
    /// </summary>
    string Code();

    /// <summary>
    /// Verifies that the definition is compatible with the provided form structure and fails fast on mismatch.
    /// </summary>
    void Verify(IEvaluationForm form);

    /// <summary>
    /// Creates a runtime calculation policy from this definition.
    /// </summary>
    ICalculationPolicy Policy();
}
