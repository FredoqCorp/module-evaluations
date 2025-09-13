using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Policies;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Runs;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Policies;

/// <summary>
/// Runtime weighted mean policy using per-node weights.
/// </summary>
public sealed record WeightedMeanPolicy : ICalculationPolicy
{
    private const string PolicyCode = "weighted-mean";
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
    public string Code() => PolicyCode;

    

    /// <summary>
    /// Calculates weighted mean with local normalization.
    /// </summary>
    public decimal Total(IRunFormSnapshot snapshot, IImmutableList<IRunCriterionScore> scores)
    {
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentNullException.ThrowIfNull(scores);

        var scoreByKey = scores
            .Where(s => !s.Skipped() && s.Assessment().Present())
            .ToDictionary(s => s.CriterionId().Value, s => (decimal)s.Assessment().SelectedScore());

        var rootCriteriaEntries = snapshot
            .Criteria()
            .Ids()
            .Where(c => scoreByKey.ContainsKey(c.Value))
            .Select(c =>
            {
                if (!_weights.TryGetValue(c.Value, out var w))
                {
                    throw new InvalidDataException("Weight is missing for root criterion in weighted policy");
                }
                var sc = scoreByKey[c.Value];
                return (score: sc, weightBps: (decimal)w.Bps());
            });

        var rootGroupEntries = snapshot
            .Groups()
            .Ids()
            .Select(g =>
            {
                var group = snapshot.Groups().Group(g.Value);
                var (any, score) = CombineGroup(group, scoreByKey);
                return (any, score, id: g.Value);
            })
            .Where(x => x.any)
            .Select(x =>
            {
                if (!_weights.TryGetValue(x.id, out var w))
                {
                    throw new InvalidDataException("Weight is missing for root group in weighted policy");
                }
                return (score: x.score, weightBps: (decimal)w.Bps());
            });

        var rootEntries = rootCriteriaEntries
            .Concat(rootGroupEntries)
            .ToList();

        if (rootEntries.Count == 0)
        {
            return 0m;
        }
        var rootSum = rootEntries.Sum(e => e.weightBps);
        if (rootSum <= 0m)
        {
            throw new InvalidDataException("Weights sum for present root siblings must be greater than zero");
        }
        var totalRoot = rootEntries.Sum(e => e.score * (e.weightBps / rootSum));
        return totalRoot;
    }

    private (bool any, decimal score) CombineGroup(FormGroup g, Dictionary<Guid, decimal> scoreByKey)
    {
        var criterionEntries = g
            .Criteria
            .Ids()
            .Where(c => scoreByKey.ContainsKey(c.Value))
            .Select(c =>
            {
                if (!_weights.TryGetValue(c.Value, out var w))
                {
                    throw new InvalidDataException("Weight is missing for criterion in weighted policy");
                }
                var sc = scoreByKey[c.Value];
                return (score: sc, weightBps: (decimal)w.Bps());
            });

        var groupEntries = g
            .Groups
            .Ids()
            .Select(childId =>
            {
                var child = g.Groups.Group(childId.Value);
                var (any, score) = CombineGroup(child, scoreByKey);
                return (any, score, child);
            })
            .Where(x => x.any)
            .Select(x =>
            {
                if (!_weights.TryGetValue(x.child.Id.Value, out var w))
                {
                    throw new InvalidDataException("Weight is missing for group in weighted policy");
                }
                return (score: x.score, weightBps: (decimal)w.Bps());
            });

        var entries = criterionEntries
            .Concat(groupEntries)
            .ToList();

        if (entries.Count == 0)
        {
            return (false, 0m);
        }
        var sumBps = entries.Sum(e => e.weightBps);
        if (sumBps <= 0m)
        {
            throw new InvalidDataException("Weights sum for present siblings must be greater than zero");
        }
        var total = entries.Sum(e => e.score * (e.weightBps / sumBps));
        return (true, total);
    }
}
