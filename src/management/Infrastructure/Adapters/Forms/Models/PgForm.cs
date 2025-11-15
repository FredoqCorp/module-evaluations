using System.Data;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Media;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Queries;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms.Models;

/// <summary>
/// Printable form aggregate backed by PostgreSQL.
/// </summary>
internal sealed class PgForm : IForm
{
    private readonly IDbConnection _connection;
    private readonly FormId _id;
    private readonly IGroups _groups;
    private readonly ICriteria _criteria;
    private readonly CancellationToken _cancellation;

    /// <summary>
    /// Initializes the printable form aggregate.
    /// </summary>
    /// <param name="connection">Database connection.</param>
    /// <param name="id">Form identifier.</param>
    /// <param name="groups">Printable groups projection.</param>
    /// <param name="criteria">Printable criteria projection.</param>
    /// <param name="cancellation">Cancellation token.</param>
    public PgForm(IDbConnection connection, FormId id, IGroups groups, ICriteria criteria, CancellationToken cancellation)
    {
        ArgumentNullException.ThrowIfNull(connection);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(criteria);

        _connection = connection;
        _id = id;
        _groups = groups;
        _criteria = criteria;
        _cancellation = cancellation;
    }

    /// <inheritdoc />
    public async Task<IMedia<TOutput>> Print<TOutput>(IMedia<TOutput> media)
    {
        ArgumentNullException.ThrowIfNull(media);
        var command = new CommandDefinition(
            FormQueries.LoadFormById,
            new { Id = _id.Value },
            cancellationToken: _cancellation);
        var row = await _connection.QuerySingleOrDefaultAsync<(Guid Id, string Name, string? Description, string Code, string Tags, string RootGroupType)>(command);
        if (row == default)
        {
            throw new InvalidOperationException("Form not found");
        }

        var metadata = new FormMetadata(
            new FormName(row.Name),
            new FormDescription(row.Description ?? string.Empty),
            new FormCode(row.Code),
            new PgJsonTags(row.Tags));

        media.With("formId", _id.Value);
        metadata.Print(media);
        media.With("calculation", row.RootGroupType);
        await _groups.Print(media);
        await _criteria.Print(media);
        return media;
    }
}
