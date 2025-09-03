using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;

/// <summary>
/// Runtime calculation policy bound to a specific run snapshot.
/// </summary>
public interface ICalculationPolicy
{
    /// <summary>
    /// Returns stable code of the policy used for serialization and logs.
    /// </summary>
    string Code();

    /// <summary>
    /// Calculates total score from the snapshot and provided per-criterion scores.
    /// </summary>
    decimal Total(IRunFormSnapshot snapshot, IImmutableList<IRunCriterionScore> scores);
}
