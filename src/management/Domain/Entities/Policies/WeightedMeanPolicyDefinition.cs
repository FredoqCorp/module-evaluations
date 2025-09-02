using System;
using System.Collections.Immutable;
using System.Linq;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Design-time definition of weighted mean policy with a mirrored tree of weights.
/// </summary>
public sealed record WeightedMeanPolicyDefinition : ICalculationPolicyDefinition
{
    private readonly IImmutableList<WeightedGroupDefinition> _groups;
    private readonly IImmutableList<WeightedCriterionDefinition> _criteria;

    /// <summary>
    /// Creates a definition with root-level criteria and groups weights.
    /// </summary>
    public WeightedMeanPolicyDefinition(IImmutableList<WeightedGroupDefinition> groups, IImmutableList<WeightedCriterionDefinition> criteria)
    {
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);
        _groups = groups;
        _criteria = criteria;
    }

    /// <summary>
    /// Returns stable policy code.
    /// </summary>
    public string Code() => "weighted-mean";

    /// <summary>
    /// Verifies that the definition is structurally compatible with the form.
    /// </summary>
    public void Verify(Interfaces.IEvaluationForm form)
    {
        ArgumentNullException.ThrowIfNull(form);

        void VerifyLevel(IImmutableList<Interfaces.IFormGroup> formGroups,
            IImmutableList<Interfaces.IFormCriterion> formCriteria,
            IImmutableList<WeightedGroupDefinition> defGroups,
            IImmutableList<WeightedCriterionDefinition> defCriteria)
        {
            if (formGroups.Count != defGroups.Count || formCriteria.Count != defCriteria.Count)
            {
                throw new InvalidDataException("Weighted policy definition structure does not match form structure");
            }

            decimal sum = 0m;
            sum += defCriteria.Sum(c => (decimal)c.Weight().Bps());
            sum += defGroups.Sum(g => (decimal)g.Weight().Bps());
            if (sum != 10_000m)
            {
                throw new InvalidDataException("Weights sum for siblings must be exactly one hundred percent");
            }

            for (int i = 0; i < formGroups.Count; i++)
            {
                var fg = formGroups[i];
                var dg = defGroups[i];
                VerifyLevel(fg.Groups(), fg.Criteria(), dg.Groups(), dg.Criteria());
            }
        }

        VerifyLevel(form.Groups(), form.Criteria(), _groups, _criteria);
    }

    /// <summary>
    /// Binds the definition to the snapshot by pairing nodes by order and returns a runtime weighted policy.
    /// </summary>
    public ICalculationPolicy Bind(IRunFormSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        var weights = ImmutableDictionary.CreateBuilder<Guid, Weight>();

        void BindGroup(IRunFormGroup g, WeightedGroupDefinition def)
        {
            // Group weight is optional in weighted mean if used hierarchically
            weights[g.Key()] = def.Weight();
            if (g.Criteria().Count != def.Criteria().Count || g.Groups().Count != def.Groups().Count)
            {
                throw new InvalidDataException("Weighted policy definition group structure does not match snapshot structure");
            }
            for (int i = 0; i < g.Criteria().Count; i++)
            {
                var rc = g.Criteria()[i];
                var cd = def.Criteria()[i];
                weights[rc.Key()] = cd.Weight();
            }
            for (int i = 0; i < g.Groups().Count; i++)
            {
                BindGroup(g.Groups()[i], def.Groups()[i]);
            }
        }

        if (snapshot.Criteria().Count != _criteria.Count || snapshot.Groups().Count != _groups.Count)
        {
            throw new InvalidDataException("Weighted policy definition root structure does not match snapshot structure");
        }

        for (int i = 0; i < snapshot.Criteria().Count; i++)
        {
            weights[snapshot.Criteria()[i].Key()] = _criteria[i].Weight();
        }
        for (int i = 0; i < snapshot.Groups().Count; i++)
        {
            BindGroup(snapshot.Groups()[i], _groups[i]);
        }

        return new WeightedMeanPolicy(weights.ToImmutable());
    }
}
