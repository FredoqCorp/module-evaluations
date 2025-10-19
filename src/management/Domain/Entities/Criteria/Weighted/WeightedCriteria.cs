using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;

/// <summary>
/// Immutable collection of weighted criteria that aggregates their contributions and sibling weight.
/// </summary>
public sealed class WeightedCriteria : IWeightedCriteria
{
    private IImmutableList<IWeightedCriterion> _items;

    /// <summary>
    /// Initializes the weighted criteria collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of weighted criteria.</param>
    public WeightedCriteria(IEnumerable<IWeightedCriterion> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Adds a new weighted criterion to the form root level.
    /// </summary>
    /// <param name="id">Identifier of the criterion.</param>
    /// <param name="text">Text description of the criterion.</param>
    /// <param name="title">Title of the criterion.</param>
    /// <param name="ratingOptions">Rating options associated with the criterion.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="weight">Weight of the criterion.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted criterion.</returns>
    public Task<IWeightedCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, FormId formId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(ratingOptions);
        ArgumentNullException.ThrowIfNull(weight);

        var baseCriterion = new Criterion(id, text, title, ratingOptions);
        var criterion = new WeightedCriterion(baseCriterion, weight);
        _items = _items.Add(criterion);

        return Task.FromResult<IWeightedCriterion>(criterion);
    }

    /// <summary>
    /// Adds a new weighted criterion to a group.
    /// </summary>
    /// <param name="id">Identifier of the criterion.</param>
    /// <param name="text">Text description of the criterion.</param>
    /// <param name="title">Title of the criterion.</param>
    /// <param name="ratingOptions">Rating options associated with the criterion.</param>
    /// <param name="groupId">Identifier of the parent group.</param>
    /// <param name="weight">Weight of the criterion.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted criterion.</returns>
    public Task<IWeightedCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, GroupId groupId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(ratingOptions);
        ArgumentNullException.ThrowIfNull(weight);

        var baseCriterion = new Criterion(id, text, title, ratingOptions);
        var criterion = new WeightedCriterion(baseCriterion, weight);
        _items = _items.Add(criterion);

        return Task.FromResult<IWeightedCriterion>(criterion);
    }

    /// <summary>
    /// Returns the combined sibling weight represented by the collection.
    /// </summary>
    /// <returns>Total weight of the criteria expressed in basis points.</returns>
    public IBasisPoints Weight()
    {
        var points = 0;

        foreach (var item in _items)
        {
            var basis = item.Weight().Percent().Basis();
            points += decimal.ToInt32(basis.Apply(10000m));

            if (points > 10000)
            {
                throw new InvalidOperationException("Criteria sibling weights exceed one hundred percent");
            }
        }

        return new BasisPoints((ushort)points);
    }

    /// <summary>
    /// Validates the internal consistency of the weighted criteria collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }

        if (_items.Count == 0)
        {
            return;
        }

        var total = Weight().Apply(10000m);

        if (total != 10000m)
        {
            throw new InvalidOperationException("Criteria sibling weights must equal one hundred percent");
        }
    }
}
