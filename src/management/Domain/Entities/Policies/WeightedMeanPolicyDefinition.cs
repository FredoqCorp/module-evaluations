using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Design-time definition of weighted mean policy with a mirrored tree of weights.
/// </summary>
public sealed record WeightedMeanPolicyDefinition : ICalculationPolicyDefinition
{
    private readonly IImmutableDictionary<Guid, Weight> _weights;

    /// <summary>
    /// Creates a definition with a flat map of weights keyed by stable form node identifiers.
    /// </summary>
    public WeightedMeanPolicyDefinition(IImmutableDictionary<Guid, Weight> weights)
    {
        ArgumentNullException.ThrowIfNull(weights);
        _weights = weights;
    }

    /// <summary>
    /// Returns stable policy code.
    /// </summary>
    public string Code() => "weighted-mean";

    /// <summary>
    /// Verifies that the definition is structurally compatible with the form.
    /// </summary>
    public void Verify(IEvaluationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);

        void VerifyLevel(IImmutableList<FormGroup> formGroups,
            IImmutableList<FormCriterion> formCriteria)
        {
            if (formGroups.Count + formCriteria.Count == 0)
            {
                return;
            }
            decimal sum = 0m;
            foreach (var c in formCriteria)
            {
                if (!_weights.TryGetValue(c.Id.Value, out var w))
                {
                    throw new InvalidDataException("Weight is missing for form criterion in weighted policy definition");
                }
                sum += w.Bps();
            }
            foreach (var g in formGroups)
            {
                if (!_weights.TryGetValue(g.Id.Value, out var w))
                {
                    throw new InvalidDataException("Weight is missing for form group in weighted policy definition");
                }
                sum += w.Bps();
            }
            if (sum != 10_000m)
            {
                throw new InvalidDataException("Weights sum for siblings must be exactly one hundred percent");
            }

            foreach (var g in formGroups)
            {
                VerifyLevel(g.Groups, g.Criteria);
            }
        }

        VerifyLevel(form.Groups(), form.Criteria());
    }

    /// <summary>
    /// Binds the definition to the snapshot by pairing nodes by order and returns a runtime weighted policy.
    /// </summary>
    public ICalculationPolicy Policy() => new WeightedMeanPolicy(_weights);
}
