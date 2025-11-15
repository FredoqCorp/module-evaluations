using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms.Models;

/// <summary>
/// Printable group built from PostgreSQL rows with nested criteria and groups.
/// </summary>
internal sealed class PgGroup : IGroup
{
    private readonly Guid _id;
    private readonly string _title;
    private readonly string _description;
    private readonly int _order;
    private readonly int? _weight;
    private readonly IReadOnlyList<ICriterion> _criteria;
    private readonly IReadOnlyList<IGroup> _children;

    /// <summary>
    /// Initializes the printable group snapshot.
    /// </summary>
    /// <param name="id">Group identifier.</param>
    /// <param name="title">Group title.</param>
    /// <param name="description">Group description.</param>
    /// <param name="order">Display order.</param>
    /// <param name="weight">Optional weight in basis points.</param>
    /// <param name="criteria">Criteria belonging to this group.</param>
    /// <param name="children">Child groups.</param>
    public PgGroup(
        Guid id,
        string title,
        string description,
        int order,
        int? weight,
        IReadOnlyList<ICriterion> criteria,
        IReadOnlyList<IGroup> children)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(children);
        ArgumentNullException.ThrowIfNull(criteria);

        _id = id;
        _title = title;
        _description = description ?? string.Empty;
        _order = order;
        _weight = weight;
        _criteria = criteria;
        _children = children;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("id", _id);
        media.With("title", _title);
        media.With("description", _description);
        media.With("orderIndex", _order);
        if (_weight.HasValue)
        {
            media.With("weightBasisPoints", _weight.Value);
        }

        media.WithArray("criteria", CriterionItems(media));
        media.WithArray("groups", GroupItems(media));
        return media;
    }

    private IEnumerable<Action<IMedia>> CriterionItems<TOutput>(IMedia<TOutput> media)
    {
        foreach (var criterion in _criteria)
        {
            yield return _ => criterion.Print(media);
        }
    }

    private IEnumerable<Action<IMedia>> GroupItems<TOutput>(IMedia<TOutput> media)
    {
        foreach (var child in _children)
        {
            yield return _ => child.Print(media);
        }
    }
}
