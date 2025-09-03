using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Runtime arithmetic mean policy that calculates simple average of available criterion scores.
/// </summary>
public sealed record ArithmeticMeanPolicy : ICalculationPolicy
{
    /// <summary>
    /// Returns stable policy code.
    /// </summary>
    public string Code() => "arithmetic-mean";

    /// <summary>
    /// Calculates arithmetic mean over non-skipped criteria that have assessments.
    /// </summary>
    public decimal Total(IRunFormSnapshot snapshot, IImmutableList<IRunCriterionScore> scores)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(scores);

        decimal sum = 0m;
        int count = 0;
        foreach (var s in scores)
        {
            if (s is null)
            {
                continue;
            }
            if (s.Skipped())
            {
                continue;
            }
            var a = s.Assessment();
            if (a is null)
            {
                continue;
            }
            sum += a.SelectedScore();
            count++;
        }
        if (count == 0)
        {
            return 0m;
        }
        return sum / count;
    }
}
