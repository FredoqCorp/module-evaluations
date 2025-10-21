using System.Collections.Generic;
using System.Collections.Immutable;
using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using CascVel.Modules.Evaluations.Management.Infrastructure.Queries;
using Dapper;
using System.Text.Json;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters;

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
    public async Task<IImmutableList<IFormSummary>> List(CancellationToken ct = default)
    {
        var connection = await _unitOfWork.ActiveConnection(ct);
        var command = new CommandDefinition(FormQueries.LoadFormSummaries, cancellationToken: ct);
        var rows = await connection.QueryAsync(command);
        var summaries = new List<IFormSummary>();
        foreach (var row in rows)
        {
            Guid guid = row.Id;
            string title = row.Name;
            string narrative = row.Description ?? string.Empty;
            string token = row.Code;
            string payload = row.Tags;
            string root = row.RootGroupType;
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
}
