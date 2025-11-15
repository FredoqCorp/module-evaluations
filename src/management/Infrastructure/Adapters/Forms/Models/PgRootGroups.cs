using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Queries;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms.Models;

/// <summary>
/// Printable groups hierarchy that loads structure and criteria in a single pass.
/// </summary>
internal sealed class PgRootGroups : IGroups
{
    private readonly IDbConnection _connection;
    private readonly FormId _form;
    private readonly CancellationToken _cancellation;

    public PgRootGroups(IDbConnection connection, FormId form, CancellationToken cancellation)
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
        var groups = await LoadGroups();
        var criteria = await LoadCriteria();
        var tree = Build(groups, criteria);
        media.WithArray("groups", Items(tree, media));
        return media;
    }

    private async Task<IReadOnlyDictionary<Guid, List<GroupRow>>> LoadGroups()
    {
        var command = new CommandDefinition(
            FormQueries.LoadFormGroups,
            new { Id = _form.Value },
            cancellationToken: _cancellation);
        var rows = await _connection.QueryAsync<(Guid Id, Guid? ParentId, string Title, string? Description, int? WeightBasisPoints, int OrderIndex)>(command);
        var map = new Dictionary<Guid, List<GroupRow>>();
        foreach (var (Id, ParentId, Title, Description, WeightBasisPoints, OrderIndex) in rows)
        {
            var parent = Key(ParentId);
            if (!map.TryGetValue(parent, out var list))
            {
                list = [];
                map[parent] = list;
            }

            list.Add(new GroupRow(
                Id,
                Title ?? string.Empty,
                Description ?? string.Empty,
                OrderIndex,
                WeightBasisPoints));
        }

        foreach (var pair in map.Values)
        {
            pair.Sort(static (left, right) => left.OrderIndex.CompareTo(right.OrderIndex));
        }

        return map;
    }

    private async Task<IReadOnlyDictionary<Guid, IReadOnlyList<ICriterion>>> LoadCriteria()
    {
        var command = new CommandDefinition(
            FormQueries.LoadFormCriteriaTree,
            new { Id = _form.Value },
            cancellationToken: _cancellation);
        var rows = await _connection.QueryAsync<(Guid Id, Guid? GroupId, string Title, string Text, int? WeightBasisPoints, int OrderIndex, string RatingOptions)>(command);
        var map = new Dictionary<Guid, List<ICriterion>>();
        foreach (var row in rows)
        {
            if (!row.GroupId.HasValue)
            {
                continue;
            }

            var key = row.GroupId.Value;
            if (!map.TryGetValue(key, out var list))
            {
                list = [];
                map[key] = list;
            }

            list.Add(new PgCriterion(
                row.Id,
                row.Title ?? string.Empty,
                row.Text ?? string.Empty,
                row.OrderIndex,
                row.WeightBasisPoints,
                new PgJsonRatingOptions(row.RatingOptions)));
        }

        var snapshot = new Dictionary<Guid, IReadOnlyList<ICriterion>>(map.Count);
        foreach (var pair in map)
        {
            pair.Value.Sort(static (left, right) => ((PgCriterion)left).Order.CompareTo(((PgCriterion)right).Order));
            snapshot[pair.Key] = pair.Value.AsReadOnly();
        }

        return snapshot;
    }

    private static List<IGroup> Build(
        IReadOnlyDictionary<Guid, List<GroupRow>> groups,
        IReadOnlyDictionary<Guid, IReadOnlyList<ICriterion>> criteria)
    {
        return BuildChildren(Guid.Empty, groups, criteria);
    }

    private static List<IGroup> BuildChildren(
        Guid parent,
        IReadOnlyDictionary<Guid, List<GroupRow>> groups,
        IReadOnlyDictionary<Guid, IReadOnlyList<ICriterion>> criteria)
    {
        if (!groups.TryGetValue(parent, out var rows))
        {
            return [];
        }

        var result = new List<IGroup>(rows.Count);
        foreach (var row in rows)
        {
            var children = BuildChildren(row.Id, groups, criteria);
            var local = criteria.TryGetValue(row.Id, out var list)
                ? list
                : [];
            result.Add(new PgGroup(
                row.Id,
                row.Title,
                row.Description,
                row.OrderIndex,
                row.Weight,
                local,
                children));
        }

        return result;
    }

    private static IEnumerable<Action<IMedia>> Items<TOutput>(IEnumerable<IGroup> groups, IMedia<TOutput> media)
    {
        foreach (var group in groups)
        {
            yield return _ => group.Print(media);
        }
    }

    private sealed record GroupRow(Guid Id, string Title, string Description, int OrderIndex, int? Weight);

    private static Guid Key(Guid? parent)
    {
        return parent ?? Guid.Empty;
    }

}
