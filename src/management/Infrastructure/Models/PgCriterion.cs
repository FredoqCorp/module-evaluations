using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Models;

/// <summary>
/// Printable criterion composed from PostgreSQL rows.
/// </summary>
internal sealed class PgCriterion : ICriterion
{
    private readonly Guid _id;
    private readonly string _title;
    private readonly string _text;
    private readonly int? _weight;
    private readonly IRatingOptions _ratings;
    private readonly int _order;

    /// <summary>
    /// Initializes the printable criterion using raw values.
    /// </summary>
    /// <param name="id">Criterion identifier.</param>
    /// <param name="title">Criterion title.</param>
    /// <param name="text">Criterion text.</param>
    /// <param name="order">Display order.</param>
    /// <param name="weight">Optional weight in basis points.</param>
    /// <param name="ratings">Rating options printer.</param>
    public PgCriterion(Guid id, string title, string text, int order, int? weight, IRatingOptions ratings)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(ratings);

        _id = id;
        _title = title;
        _text = text ?? string.Empty;
        _order = order;
        _weight = weight;
        _ratings = ratings;
    }

    /// <inheritdoc />
    public IMedia<TOutput> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        media.With("id", _id);
        media.With("title", _title);
        media.With("text", _text);
        media.With("orderIndex", _order);
        if (_weight.HasValue)
        {
            media.With("weightBasisPoints", _weight.Value);
        }

        _ratings.Print(media);
        return media;
    }
}
