using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;

/// <summary>
/// Immutable collection of unweighted criteria that participate in average scoring.
/// </summary>
public sealed class AverageCriteria : IAverageCriteria
{
    private IImmutableList<IAverageCriterion> _items;

    /// <summary>
    /// Initializes the unweighted criteria collection from the provided sequence.
    /// </summary>
    /// <param name="items">Sequence of unweighted criteria.</param>
    public AverageCriteria(IEnumerable<IAverageCriterion> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
    }

    /// <summary>
    /// Adds a new unweighted criterion to the form root level.
    /// </summary>
    /// <param name="id">Identifier of the criterion.</param>
    /// <param name="text">Text description of the criterion.</param>
    /// <param name="title">Title of the criterion.</param>
    /// <param name="ratingOptions">Rating options associated with the criterion.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added unweighted criterion.</returns>
    public Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, FormId formId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(ratingOptions);

        var criterion = new Criterion(id, text, title, ratingOptions);
        _items = _items.Add(criterion);

        return Task.FromResult<IAverageCriterion>(criterion);
    }

    /// <summary>
    /// Adds a new unweighted criterion to a group.
    /// </summary>
    /// <param name="id">Identifier of the criterion.</param>
    /// <param name="text">Text description of the criterion.</param>
    /// <param name="title">Title of the criterion.</param>
    /// <param name="ratingOptions">Rating options associated with the criterion.</param>
    /// <param name="groupId">Identifier of the parent group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added unweighted criterion.</returns>
    public Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, GroupId groupId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(ratingOptions);

        var criterion = new Criterion(id, text, title, ratingOptions);
        _items = _items.Add(criterion);

        return Task.FromResult<IAverageCriterion>(criterion);
    }

    /// <summary>
    /// Validates the internal consistency of the unweighted criteria collection.
    /// </summary>
    public void Validate()
    {
        foreach (var item in _items)
        {
            item.Validate();
        }
    }
}
