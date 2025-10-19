using System.Collections.Immutable;
using System.Data;
using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;
using Npgsql;

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
        var formSummaryRows = await connection.QueryAsync<FormSummaryRow>(
            new CommandDefinition(FormQueries.LoadFormSummaries, cancellationToken: ct));

        var summaries = new List<IFormSummary>();
        foreach (var row in formSummaryRows)
        {
            var summary = FormAssembler.AssembleSummaryFromRow(row);
            summaries.Add(summary);
        }

        return summaries.ToImmutableList();
    }
}
