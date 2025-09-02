using System;
using System.Collections.Immutable;
using System.Linq;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Runtime weighted mean policy using optional per-node weights, defaults to equal weights when missing.
/// </summary>
public sealed record WeightedMeanPolicy : ICalculationPolicy
{
    private readonly IImmutableDictionary<Guid, Weight> _weights;

    /// <summary>
    /// Creates a weighted mean policy with per-node weights map.
    /// </summary>
    public WeightedMeanPolicy(IImmutableDictionary<Guid, Weight> weights)
    {
        ArgumentNullException.ThrowIfNull(weights);
        _weights = weights;
    }

    /// <summary>
    /// Returns stable policy code.
    /// </summary>
    public string Code() => "weighted-mean";

    /// <summary>
    /// Verifies that the bound policy is compatible with the snapshot.
    /// </summary>
    public void Verify(IRunFormSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        void VerifyLevel(IImmutableList<IRunFormGroup> groups, IImmutableList<IRunFormCriterion> criteria)
        {
            decimal sum = 0m;
            foreach (var c in criteria)
            {
                if (!_weights.TryGetValue(c.Key(), out var w))
                {
                    throw new InvalidDataException("Weight is missing for criterion in weighted policy");
                }
                sum += w.Bps();
            }
            foreach (var g in groups)
            {
                if (!_weights.TryGetValue(g.Key(), out var w))
                {
                    throw new InvalidDataException("Weight is missing for group in weighted policy");
                }
                sum += w.Bps();
            }
            if (sum != 10_000m)
            {
                throw new InvalidDataException("Weights sum for siblings must be exactly one hundred percent");
            }

            foreach (var g in groups)
            {
                VerifyLevel(g.Groups(), g.Criteria());
            }
        }

        VerifyLevel(snapshot.Groups(), snapshot.Criteria());
    }

    /// <summary>
    /// Calculates weighted mean with local normalization and equal weights fallback when not provided.
    /// </summary>
    public decimal Total(IRunFormSnapshot snapshot, IImmutableList<IRunCriterionScore> scores)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(scores);

        var scoreByKey = scores
            .Where(s => s is not null && !s.Skipped() && s.Assessment() is not null)
            .ToDictionary(s => s.Criterion().Key(), s => (decimal)s.Assessment()!.SelectedScore());

        (bool any, decimal score) CombineGroup(IRunFormGroup g)
        {
            var entries = new List<(decimal score, decimal weightBps)>();

            foreach (var c in g.Criteria())
            {
                if (scoreByKey.TryGetValue(c.Key(), out var sc))
                {
                    if (!_weights.TryGetValue(c.Key(), out var w))
                    {
                        throw new InvalidDataException("Weight is missing for criterion in weighted policy");
                    }
                    entries.Add((sc, w.Bps()));
                }
            }
            foreach (var child in g.Groups())
            {
                var res = CombineGroup(child);
                if (res.any)
                {
                    if (!_weights.TryGetValue(child.Key(), out var w))
                    {
                        throw new InvalidDataException("Weight is missing for group in weighted policy");
                    }
                    entries.Add((res.score, w.Bps()));
                }
            }

            if (entries.Count == 0)
            {
                return (false, 0m);
            }
            decimal sumBps = entries.Sum(e => e.weightBps);
            if (sumBps <= 0m)
            {
                throw new InvalidDataException("Weights sum for present siblings must be greater than zero");
            }
            decimal total = 0m;
            foreach (var e in entries)
            {
                total += e.score * (e.weightBps / sumBps);
            }
            return (true, total);
        }

        var rootEntries = new List<(decimal score, decimal weightBps)>();
        foreach (var c in snapshot.Criteria())
        {
            if (scoreByKey.TryGetValue(c.Key(), out var sc))
            {
                if (!_weights.TryGetValue(c.Key(), out var w))
                {
                    throw new InvalidDataException("Weight is missing for root criterion in weighted policy");
                }
                rootEntries.Add((sc, w.Bps()));
            }
        }
        foreach (var g in snapshot.Groups())
        {
            var (any, score) = CombineGroup(g);
            if (any)
            {
                if (!_weights.TryGetValue(g.Key(), out var w))
                {
                    throw new InvalidDataException("Weight is missing for root group in weighted policy");
                }
                rootEntries.Add((score, w.Bps()));
            }
        }

        if (rootEntries.Count == 0)
        {
            return 0m;
        }
        decimal rootSum = rootEntries.Sum(e => e.weightBps);
        if (rootSum <= 0m)
        {
            throw new InvalidDataException("Weights sum for present root siblings must be greater than zero");
        }
        decimal totalRoot = 0m;
        foreach (var (score, weightBps) in rootEntries)
        {
            totalRoot += score * (weightBps / rootSum);
        }
        return totalRoot;
    }
}
