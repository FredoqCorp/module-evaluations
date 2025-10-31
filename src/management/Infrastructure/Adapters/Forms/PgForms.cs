using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
using CascVel.Modules.Evaluations.Management.Infrastructure.Queries;
using Dapper;
using Npgsql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;

/// <summary>
/// PostgreSQL implementation for loading evaluation forms.
/// </summary>
internal sealed class PgForms : IForms
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes the adapter with the database unit of work.
    /// </summary>
    /// <param name="unitOfWork">Unit of work for managing database connections and transactions.</param>
    public PgForms(IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<IForm> Add(IForm form, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(form);

        var connection = await _unitOfWork.ActiveConnection(ct);
        var stamp = DateTimeOffset.UtcNow;
        var media = new PgInsertFormMedia(stamp);
        form.Validate();
        form.Print(media);
        var script = media.Output();

        await _unitOfWork.BeginAsync(ct);
        try
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    commandText: script,
                    cancellationToken: ct));
            await _unitOfWork.CommitAsync(ct);
        }
        catch (PostgresException ex)
        {
            await _unitOfWork.RollbackAsync(ct);
            throw MapPersistenceException(ex);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(ct);
            throw;
        }

        return form;
    }

    /// <inheritdoc />
    public async Task<IImmutableList<IFormSummary>> List(CancellationToken ct = default)
    {
        var connection = await _unitOfWork.ActiveConnection(ct);
        var command = new CommandDefinition(FormQueries.LoadFormSummaries, cancellationToken: ct);
        var rows = await connection.QueryAsync<(Guid Id, string Name, string? Description, string Code, string Tags, string RootGroupType, long GroupsCount, long CriteriaCount)>(command);
        var summaries = new List<IFormSummary>();
        foreach (var row in rows)
        {
            var guid = row.Id;
            var title = row.Name;
            var narrative = row.Description ?? string.Empty;
            var token = row.Code;
            var payload = row.Tags;
            var root = row.RootGroupType;
            var id = new FormId(guid);
            var name = new FormName(title);
            var description = new FormDescription(narrative);
            var code = new FormCode(token);
            using var document = JsonDocument.Parse(payload);
            var bag = new List<Tag>();
            if (document.RootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var node in document.RootElement.EnumerateArray())
                {
                    var text = node.GetString() ?? string.Empty;
                    if (text.Length > 0)
                    {
                        bag.Add(new Tag(text));
                    }
                }
            }
            var tags = new Tags(bag);
            var metadata = new FormMetadata(name, description, code, tags);
            var calculation = Calculation(root);
            var groups = checked(Convert.ToInt32(row.GroupsCount));
            var criteria = checked(Convert.ToInt32(row.CriteriaCount));
            var summary = new FormSummary(metadata: metadata, id: id, calculationType: calculation, groupsCount: groups, criteriaCount: criteria);
            summaries.Add(summary);
        }

        return summaries.ToImmutableList();
    }

    
    
    /// <summary>
    /// Provides calculation type for a database root group discriminator.
    /// </summary>
    /// <param name="value">Database discriminator value.</param>
    /// <returns>Calculation type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when value is unknown.</exception>
    private static CalculationType Calculation(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (string.Equals(value, "average", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.Average;
        }

        if (string.Equals(value, "weighted", StringComparison.OrdinalIgnoreCase))
        {
            return CalculationType.WeightedAverage;
        }

        throw new InvalidOperationException("Unknown root group type value");
    }

    /// <summary>
    /// Maps database errors to descriptive exceptions.
    /// </summary>
    /// <param name="exception">Underlying PostgreSQL exception.</param>
    /// <returns>Translated exception.</returns>
    private static Exception MapPersistenceException(PostgresException exception)
    {
        if (exception.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return new InvalidOperationException("Database rejected form because code already exists", exception);
        }

        if (exception.SqlState == PostgresErrorCodes.ForeignKeyViolation)
        {
            return new InvalidOperationException("Database rejected form because referenced parents are missing", exception);
        }

        if (exception.SqlState == PostgresErrorCodes.CheckViolation)
        {
            return new InvalidOperationException("Database rejected form because structural constraints failed", exception);
        }

        return exception;
    }
}
