using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Runtime weighted mean policy using optional per-node weights, defaults to equal weights when missing.
/// </summary>
public sealed record WeightedMeanPolicy : ICalculationPolicy
{
    private readonly IImmutableDictionary<string, Forms.ValueObjects.Weight> _weights;

    /// <summary>
    /// Creates a weighted mean policy with per-node weights map.
    /// </summary>
    public WeightedMeanPolicy(IImmutableDictionary<string, Forms.ValueObjects.Weight> weights)
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
            .Where(s => !s.Skipped() && s.Assessment().Present())
            .ToDictionary(s => s.Criterion().Id().Text(), s => (decimal)s.Assessment().SelectedScore());

        var rootEntries = new List<(decimal score, decimal weightBps)>();
        foreach (var c in snapshot.Criteria())
        {
            if (!scoreByKey.TryGetValue(c.Id().Text(), out var sc))
            {
                continue;
            }
            if (!_weights.TryGetValue(c.Id().Text(), out var w))
            {
                throw new InvalidDataException("Weight is missing for root criterion in weighted policy");
            }
            rootEntries.Add((sc, w.Bps()));
        }
        foreach (var g in snapshot.Groups())
        {
            var (any, score) = CombineGroup(g, scoreByKey);
            if (!any)
            {
                continue;
            }
            if (!_weights.TryGetValue(g.Id().Text(), out var w))
            {
                throw new InvalidDataException("Weight is missing for root group in weighted policy");
            }
            rootEntries.Add((score, w.Bps()));
        }

        if (rootEntries.Count == 0)
        {
            return 0m;
        }
        var rootSum = rootEntries.Sum(e => e.weightBps);
        if (rootSum <= 0m)
        {
            throw new InvalidDataException("Weights sum for present root siblings must be greater than zero");
        }
        var totalRoot = 0m;
        foreach (var (score, weightBps) in rootEntries)
        {
            totalRoot += score * (weightBps / rootSum);
        }
        return totalRoot;
    }

    private (bool any, decimal score) CombineGroup(IFormGroup g, Dictionary<string, decimal> scoreByKey)
    {
        var entries = new List<(decimal score, decimal weightBps)>();

        foreach (var c in g.Criteria())
        {
            if (!scoreByKey.TryGetValue(c.Id().Text(), out var sc))
            {
                continue;
            }
            if (!_weights.TryGetValue(c.Id().Text(), out var w))
            {
                throw new InvalidDataException("Weight is missing for criterion in weighted policy");
            }
            entries.Add((sc, w.Bps()));
        }
        foreach (var child in g.Groups())
        {
            var (any, score) = CombineGroup(child, scoreByKey);
            if (!any)
            {
                continue;
            }
            if (!_weights.TryGetValue(child.Id().Text(), out var w))
            {
                throw new InvalidDataException("Weight is missing for group in weighted policy");
            }
            entries.Add((score, w.Bps()));
        }

        if (entries.Count == 0)
        {
            return (false, 0m);
        }
        var sumBps = entries.Sum(e => e.weightBps);
        if (sumBps <= 0m)
        {
            throw new InvalidDataException("Weights sum for present siblings must be greater than zero");
        }
        var total = 0m;
        foreach (var (score, weightBps) in entries)
        {
            total += score * (weightBps / sumBps);
        }
        return (true, total);
    }
}
