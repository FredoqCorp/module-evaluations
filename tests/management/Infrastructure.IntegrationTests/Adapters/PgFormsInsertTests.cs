using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Host.Models;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;
using Npgsql;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration tests that validate PgForms persistence behavior.
/// </summary>
public sealed class PgFormsInsertTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    /// <summary>
    /// Initializes the test suite with the shared database fixture.
    /// </summary>
    /// <param name="fixture">Database fixture providing PostgreSQL access.</param>
    public PgFormsInsertTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Given_weighted_payload_When_pg_forms_add_executes_Then_persist_basis_points()
    {
        var randomness = Guid.NewGuid();
        var code = $"code-{randomness:N}";
        await using var setupConnection = new NpgsqlConnection(_fixture.ConnectionString);
        await setupConnection.OpenAsync();
        await setupConnection.ExecuteAsync("DELETE FROM form_criteria WHERE form_id IN (SELECT id FROM forms WHERE code = @Code)", new { Code = code });
        await setupConnection.ExecuteAsync("DELETE FROM form_groups WHERE form_id IN (SELECT id FROM forms WHERE code = @Code)", new { Code = code });
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE code = @Code", new { Code = code });

        var name = $"Форма-{Guid.NewGuid():N}";
        var description = $"説明-{Guid.NewGuid():N}";
        var groupTitle = $"Группа-{Guid.NewGuid():N}";
        var groupDescription = $"Detalhe-{Guid.NewGuid():N}";
        var criterionTitle = $"Κριτήριο-{Guid.NewGuid():N}";
        var criterionText = $"Текст-{Guid.NewGuid():N}";
        var ratingLabel = $"Лейбл-{Guid.NewGuid():N}";
        var ratingAnnotation = $"注釈-{Guid.NewGuid():N}";
        const decimal groupWeight = 44.5m;
        const decimal criterionWeight = 55.5m;
        const int expectedGroupWeight = 4450;
        const int expectedCriterionWeight = 5550;

        var payload = $@"
{{
  ""metadata"": {{
    ""name"": ""{name}"",
    ""description"": ""{description}"",
    ""code"": ""{code}"",
    ""tags"": [""тег-{Guid.NewGuid():N}"", ""β-{Guid.NewGuid():N}""]
  }},
  ""calculation"": ""weighted"",
  ""root"": {{
    ""criteria"": [
      {{
        ""title"": ""{criterionTitle}"",
        ""text"": ""{criterionText}"",
        ""order"": 0,
        ""weight"": {criterionWeight.ToString(System.Globalization.CultureInfo.InvariantCulture)},
        ""ratingOptions"": [
          {{ ""order"": 0, ""score"": 5, ""label"": ""{ratingLabel}"", ""annotation"": ""{ratingAnnotation}"" }}
        ]
      }}
    ],
    ""groups"": [
      {{
        ""title"": ""{groupTitle}"",
        ""description"": ""{groupDescription}"",
        ""order"": 1,
        ""weight"": {groupWeight.ToString(System.Globalization.CultureInfo.InvariantCulture)},
        ""criteria"": [],
        ""groups"": []
      }}
    ]
  }}
}}";

        using var document = JsonDocument.Parse(payload);
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var adapter = new PgForms(uow);
        var form = new JsonNewForm(document);

        var verificationTask = Task.Run(async () =>
        {
            await Task.Delay(25);
            await using var checkConnection = new NpgsqlConnection(_fixture.ConnectionString);
            await checkConnection.OpenAsync();
            var exists = await checkConnection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM forms WHERE code = @Code",
                new { Code = code });
            var groupWeightPoints = await checkConnection.QuerySingleOrDefaultAsync<int?>(
                "SELECT weight_basis_points FROM form_groups WHERE form_id = (SELECT id FROM forms WHERE code = @Code)",
                new { Code = code });
            var criterionWeightPoints = await checkConnection.QuerySingleOrDefaultAsync<int?>(
                "SELECT weight_basis_points FROM form_criteria WHERE form_id = (SELECT id FROM forms WHERE code = @Code)",
                new { Code = code });
            return exists == 1 && groupWeightPoints == expectedGroupWeight && criterionWeightPoints == expectedCriterionWeight;
        });

        await adapter.Add(form);
        var success = await verificationTask;
        success.ShouldBeTrue("Rows were not inserted as expected");
    }

    [Fact]
    public async Task Given_average_payload_When_pg_forms_add_executes_Then_skip_weights()
    {
        var randomness = Guid.NewGuid();
        var code = $"avg-{randomness:N}";
        await using var setupConnection = new NpgsqlConnection(_fixture.ConnectionString);
        await setupConnection.OpenAsync();
        await setupConnection.ExecuteAsync("DELETE FROM form_criteria WHERE form_id IN (SELECT id FROM forms WHERE code = @Code)", new { Code = code });
        await setupConnection.ExecuteAsync("DELETE FROM form_groups WHERE form_id IN (SELECT id FROM forms WHERE code = @Code)", new { Code = code });
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE code = @Code", new { Code = code });

        var name = $"Оценка-{Guid.NewGuid():N}";
        var description = $"descr-{Guid.NewGuid():N}";
        var groupTitle = $"Grupo-{Guid.NewGuid():N}";
        var criterionTitle = $"Critério-{Guid.NewGuid():N}";
        var criterionText = $"Texto-{Guid.NewGuid():N}";
        var ratingLabel = $"Etiqueta-{Guid.NewGuid():N}";
        var ratingAnnotation = $"Αν{Guid.NewGuid():N}";

        var payload = $@"
{{
  ""metadata"": {{
    ""name"": ""{name}"",
    ""description"": ""{description}"",
    ""code"": ""{code}"",
    ""tags"": [""ξενικό-{Guid.NewGuid():N}""]
  }},
  ""calculation"": ""average"",
  ""root"": {{
    ""criteria"": [],
    ""groups"": [
      {{
        ""title"": ""{groupTitle}"",
        ""description"": ""{description}"",
        ""order"": 0,
        ""criteria"": [
          {{
            ""title"": ""{criterionTitle}"",
            ""text"": ""{criterionText}"",
            ""order"": 0,
            ""ratingOptions"": [
              {{ ""order"": 0, ""score"": 4, ""label"": ""{ratingLabel}"", ""annotation"": ""{ratingAnnotation}"" }}
            ]
          }}
        ],
        ""groups"": []
      }}
    ]
  }}
}}";

        using var document = JsonDocument.Parse(payload);
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var adapter = new PgForms(uow);
        var form = new JsonNewForm(document);

        var verificationTask = Task.Run(async () =>
        {
            await Task.Delay(25);
            await using var checkConnection = new NpgsqlConnection(_fixture.ConnectionString);
            await checkConnection.OpenAsync();
            var exists = await checkConnection.QuerySingleAsync<int>(
                "SELECT COUNT(*) FROM forms WHERE code = @Code",
                new { Code = code });
            var groupWeightPoints = await checkConnection.QuerySingleOrDefaultAsync<int?>(
                "SELECT weight_basis_points FROM form_groups WHERE form_id = (SELECT id FROM forms WHERE code = @Code)",
                new { Code = code });
            var criterionWeightPoints = await checkConnection.QuerySingleOrDefaultAsync<int?>(
                "SELECT weight_basis_points FROM form_criteria WHERE form_id = (SELECT id FROM forms WHERE code = @Code)",
                new { Code = code });
            return exists == 1 && groupWeightPoints is null && criterionWeightPoints is null;
        });

        await adapter.Add(form);
        var success = await verificationTask;
        success.ShouldBeTrue("Average form rows recorded unexpected weights");
    }
}
