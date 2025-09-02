using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using System.Collections.Immutable;

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
    void Verify(Interfaces.IEvaluationForm form);

    /// <summary>
    /// Binds the definition to a run snapshot and returns a runtime policy instance.
    /// </summary>
    ICalculationPolicy Bind(IRunFormSnapshot snapshot);
}

