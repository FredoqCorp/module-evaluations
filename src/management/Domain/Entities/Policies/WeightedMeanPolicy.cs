using System;
using System.Collections.Immutable;
using System.Linq;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Runtime weighted mean policy using optional per-node weights, defaults to equal weights when missing.
/// </summary>
public sealed record WeightedMeanPolicy : ICalculationPolicy
{
    private readonly IImmutableDictionary<string, CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.Weight> _weights;

    /// <summary>
    /// Creates a weighted mean policy with per-node weights map.
    /// </summary>
    public WeightedMeanPolicy(IImmutableDictionary<string, CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.ValueObjects.Weight> weights)
    {
        ArgumentNullException.ThrowIfNull(weights);
        _weights = weights;
    }

    /// <summary>
    /// Returns stable policy code.
    /// </summary>
    public string Code() => "weighted-mean";

    

    /// <summary>
    /// Calculates weighted mean with local normalization and equal weights fallback when not provided.
    /// </summary>
    public decimal Total(IRunFormSnapshot snapshot, IImmutableList<IRunCriterionScore> scores)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(scores);

        var scoreByKey = scores
            .Where(s => s is not null && !s.Skipped() && s.Assessment() is not null)
            .ToDictionary(s => s.Criterion().Id().Text(), s => (decimal)s.Assessment()!.SelectedScore());

        (bool any, decimal score) CombineGroup(IRunFormGroup g)
        {
            var entries = new List<(decimal score, decimal weightBps)>();

            foreach (var c in g.Criteria())
            {
                if (scoreByKey.TryGetValue(c.Id().Text(), out var sc))
                {
                    if (!_weights.TryGetValue(c.Id().Text(), out var w))
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
                    if (!_weights.TryGetValue(child.Id().Text(), out var w))
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
            if (scoreByKey.TryGetValue(c.Id().Text(), out var sc))
            {
                if (!_weights.TryGetValue(c.Id().Text(), out var w))
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
                if (!_weights.TryGetValue(g.Id().Text(), out var w))
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
