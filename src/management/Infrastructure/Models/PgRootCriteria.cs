using System.Data;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Queries;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Models;

/// <summary>
/// Printable criteria collection for root-level criteria.
/// </summary>
internal sealed class PgRootCriteria : ICriteria
{
    private readonly IDbConnection _connection;
    private readonly FormId _form;
    private readonly CancellationToken _cancellation;

    public PgRootCriteria(IDbConnection connection, FormId form, CancellationToken cancellation)
    {
        ArgumentNullException.ThrowIfNull(connection);
        _connection = connection;
        _form = form;
        _cancellation = cancellation;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        var command = new CommandDefinition(
            FormQueries.LoadRootCriteria,
            new { Id = _form.Value },
            cancellationToken: _cancellation);
        var rows = await _connection.QueryAsync<(Guid Id, string Title, string Text, int? WeightBasisPoints, int OrderIndex, string RatingOptions)>(command);
        var criteria = rows
            .Select(row => new PgCriterion(
                row.Id,
                row.Title ?? string.Empty,
                row.Text ?? string.Empty,
                row.OrderIndex,
                row.WeightBasisPoints,
                new PgJsonRatingOptions(row.RatingOptions)))
            .ToList();

        await media.WithArrayAsync("criteria", Items(criteria, media));
        return media;
    }

    private static IEnumerable<Func<IMedia, Task>> Items<TOutput>(IEnumerable<PgCriterion> criteria, IMedia<TOutput> media)
    {
        foreach (var criterion in criteria)
        {
            yield return _ => criterion.Print(media);
        }
    }
}
