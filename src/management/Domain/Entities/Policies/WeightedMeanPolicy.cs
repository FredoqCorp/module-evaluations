using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Runtime weighted mean policy using per-node weights.
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
    /// Calculates weighted mean with local normalization.
    /// </summary>
    public decimal Total(IRunFormSnapshot snapshot, IImmutableList<IRunCriterionScore> scores)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(scores);

        var scoreByKey = scores
            .Where(s => !s.Skipped() && s.Assessment().Present())
            .ToDictionary(s => s.Criterion().Id.Value, s => (decimal)s.Assessment().SelectedScore());

        var rootEntries = new List<(decimal score, decimal weightBps)>();
        foreach (var c in snapshot.Criteria())
        {
            if (!scoreByKey.TryGetValue(c.Id.Value, out var sc))
            {
                continue;
            }
            if (!_weights.TryGetValue(c.Id .Value, out var w))
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
            if (!_weights.TryGetValue(g.Id.Value, out var w))
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

    private (bool any, decimal score) CombineGroup(FormGroup g, Dictionary<Guid, decimal> scoreByKey)
    {
        var entries = new List<(decimal score, decimal weightBps)>();

        foreach (var c in g.Criteria)
        {
            if (!scoreByKey.TryGetValue(c.Id.Value, out var sc))
            {
                continue;
            }
            if (!_weights.TryGetValue(c.Id.Value, out var w))
            {
                throw new InvalidDataException("Weight is missing for criterion in weighted policy");
            }
            entries.Add((sc, w.Bps()));
        }
        foreach (var child in g.Groups)
        {
            var (any, score) = CombineGroup(child, scoreByKey);
            if (!any)
            {
                continue;
            }
            if (!_weights.TryGetValue(child.Id.Value, out var w))
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
