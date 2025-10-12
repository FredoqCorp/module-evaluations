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
internal sealed class PostgresForms : IForms
{
    private readonly IUnitOfWork _unitOfWork;

    public PostgresForms(IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);

        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public async Task<IImmutableList<IForm>> List(CancellationToken ct = default)
    {
        var connection = await _unitOfWork.ActiveConnection(ct);
        var formRows = await connection.QueryAsync<FormRow>(
            new CommandDefinition(FormQueries.LoadForms, cancellationToken: ct));
        var groupRows = await connection.QueryAsync<GroupRow>(
            new CommandDefinition(FormQueries.LoadGroups, cancellationToken: ct));
        var criteriaRows = await connection.QueryAsync<CriterionRow>(
            new CommandDefinition(FormQueries.LoadCriteria, cancellationToken: ct));
        var ratingRows = await connection.QueryAsync<RatingOptionRow>(
            new CommandDefinition(FormQueries.LoadRatings, cancellationToken: ct));

        var groupsByFormId = groupRows.GroupBy(g => g.FormId).ToDictionary(g => g.Key, g => g.ToList());
        var criteriaByFormId = criteriaRows.GroupBy(c => c.FormId).ToDictionary(g => g.Key, g => g.ToList());
        var ratingsByCriterionId = ratingRows.GroupBy(r => r.CriterionId).ToDictionary(g => g.Key, g => g.ToList());

        var assembler = new FormAssembler(ratingsByCriterionId);

        var forms = new List<IForm>();
        foreach (var formRow in formRows)
        {
            var groups = groupsByFormId.GetValueOrDefault(formRow.Id, []);
            var criteria = criteriaByFormId.GetValueOrDefault(formRow.Id, []);

            var form = assembler.Assemble(formRow, groups, criteria);
            forms.Add(form);
        }

        return forms.ToImmutableList();
    }
}
