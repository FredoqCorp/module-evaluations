using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Criteria;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
using Dapper;
using Npgsql;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration coverage for PgAverageCriteria persistence logic.
/// </summary>
public sealed class PgAverageCriteriaTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    /// <summary>
    /// Builds the test suite with a shared database fixture.
    /// </summary>
    /// <param name="fixture">Shared PostgreSQL container fixture.</param>
    public PgAverageCriteriaTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    /// <inheritdoc />
    [Fact(DisplayName = "PgAverageCriteria adds average criteria for form and group under concurrent load")]
    public async Task PgAverageCriteria_adds_average_criteria_for_form_and_group_under_concurrent_load()
    {
        var firstCriterion = new CriterionId(Guid.NewGuid());
        var secondCriterion = new CriterionId(Guid.NewGuid());
        var firstForm = new FormId(Guid.NewGuid());
        var secondForm = new FormId(Guid.NewGuid());
        var group = new GroupId(Guid.NewGuid());
        var firstTitle = new CriterionTitle($"Título-{Guid.NewGuid():N}溝");
        var firstText = new CriterionText($"Текст-{Guid.NewGuid():N}ñ");
        var secondTitle = new CriterionTitle($"Заголовок-{Guid.NewGuid():N}ø");
        var secondText = new CriterionText($"Описание-{Guid.NewGuid():N}æ");
        var firstIndex = new OrderIndex(RandomNumberGenerator.GetInt32(1, int.MaxValue));
        var secondIndex = new OrderIndex(RandomNumberGenerator.GetInt32(1, int.MaxValue));
        var firstOptions = new RatingOptions(new[]
        {
            new RatingOption(new RatingScore((ushort)RandomNumberGenerator.GetInt32(1, 4001)), new RatingLabel("ЛучшеÉ"), new RatingAnnotation("Åналогия")),
            new RatingOption(new RatingScore((ushort)RandomNumberGenerator.GetInt32(4001, 6001)), new RatingLabel("Śредне"), new RatingAnnotation("说明"))
        });
        var secondOptions = new RatingOptions(new[]
        {
            new RatingOption(new RatingScore((ushort)RandomNumberGenerator.GetInt32(1, 5001)), new RatingLabel("Óценка"), new RatingAnnotation("記述"))
        });
        var moment = DateTimeOffset.UtcNow;
        await using (var preparation = new PostgresUnitOfWork(_fixture.ConnectionString))
        {
            var connection = await preparation.ActiveConnection();
            await connection.ExecuteAsync("INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)", new { Id = firstForm.Value, Name = $"Форма-{Guid.NewGuid():N}ß", Description = $"Описание-{Guid.NewGuid():N}î", Code = $"CODE-{RandomNumberGenerator.GetInt32(100, 999)}", Tags = "[]", RootGroupType = "average", CreatedAt = moment, UpdatedAt = moment });
            await connection.ExecuteAsync("INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)", new { Id = secondForm.Value, Name = $"Форма-{Guid.NewGuid():N}ł", Description = $"Описание-{Guid.NewGuid():N}ū", Code = $"CODE-{RandomNumberGenerator.GetInt32(1000, 1999)}", Tags = "[]", RootGroupType = "average", CreatedAt = moment, UpdatedAt = moment });
            await connection.ExecuteAsync("INSERT INTO form_groups (id, form_id, parent_id, title, description, group_type, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @GroupType, @WeightBasisPoints, @OrderIndex, @CreatedAt)", new { Id = group.Value, FormId = secondForm.Value, ParentId = (Guid?)null, Title = $"Группа-{Guid.NewGuid():N}ž", Description = $"Текст-{Guid.NewGuid():N}ø", GroupType = "average", WeightBasisPoints = (int?)null, OrderIndex = RandomNumberGenerator.GetInt32(1, int.MaxValue), CreatedAt = moment });
        }
        string firstJson;
        using (var media = new JsonMediaWriter())
        {
            firstOptions.Print(media);
            firstJson = media.Output();
        }
        string secondJson;
        using (var media = new JsonMediaWriter())
        {
            secondOptions.Print(media);
            secondJson = media.Output();
        }
        var tasks = new[]
        {
            Task.Run(async () =>
            {
                await using var unit = new PostgresUnitOfWork(_fixture.ConnectionString);
                var adapter = new PgAverageCriteria(unit);
                await adapter.Add(firstCriterion, firstText, firstTitle, firstOptions, firstForm, firstIndex);
            }),
            Task.Run(async () =>
            {
                await using var unit = new PostgresUnitOfWork(_fixture.ConnectionString);
                var adapter = new PgAverageCriteria(unit);
                await adapter.Add(secondCriterion, secondText, secondTitle, secondOptions, group, secondIndex);
            })
        };
        await Task.WhenAll(tasks);
        await using var verifier = new PostgresUnitOfWork(_fixture.ConnectionString);
        var view = await verifier.ActiveConnection();
        var attempts = 0;
        List<(Guid, Guid?, Guid?, string, int?, string, int)> rows;
        while (true)
        {
            try
            {
                rows = (await view.QueryAsync<(Guid, Guid?, Guid?, string, int?, string, int)>("SELECT id, form_id, group_id, criterion_type AS type, weight_basis_points AS weight, rating_options AS options, order_index FROM form_criteria WHERE id IN (@First, @Second)", new { First = firstCriterion.Value, Second = secondCriterion.Value })).ToList();
                break;
            }
            catch (PostgresException) when (attempts < 3)
            {
                attempts++;
                await Task.Delay(50);
            }
        }
        var actual = rows
            .Select(row => new CriterionSnapshot(
                row.Item1,
                row.Item2,
                row.Item3,
                row.Item4,
                row.Item5,
                Normalize(row.Item6),
                row.Item7))
            .OrderBy(snapshot => snapshot.Id)
            .ToArray();
        var expected = new[]
        {
            new CriterionSnapshot(
                firstCriterion.Value,
                firstForm.Value,
                null,
                "average",
                null,
                Normalize(firstJson),
                firstIndex.Value),
            new CriterionSnapshot(
                secondCriterion.Value,
                secondForm.Value,
                group.Value,
                "average",
                null,
                Normalize(secondJson),
                secondIndex.Value)
        }
        .OrderBy(snapshot => snapshot.Id)
        .ToArray();
        actual.ShouldBe(expected, "database snapshot is not aligned with expectations");
    }

    /// <summary>
    /// Materializes criterion data snapshot for equality comparisons.
    /// </summary>
    private sealed record CriterionSnapshot(
        Guid Id,
        Guid? FormId,
        Guid? GroupId,
        string Type,
        int? Weight,
        string Options,
        int OrderIndex);

    /// <summary>
    /// Normalizes JSON representation to invariant canonical form.
    /// </summary>
    /// <param name="json">Original JSON string.</param>
    /// <returns>Canonical JSON representation.</returns>
    private static string Normalize(string json)
    {
        using var document = JsonDocument.Parse(json);
        var entries = new List<string>();
        foreach (var property in document.RootElement.EnumerateObject())
        {
            var score = property.Value.GetProperty("score").GetInt32().ToString(CultureInfo.InvariantCulture);
            var label = property.Value.GetProperty("label").GetString() ?? string.Empty;
            var annotation = property.Value.GetProperty("annotation").GetString() ?? string.Empty;
            entries.Add(string.Join(":", property.Name, score, label, annotation));
        }
        entries.Sort(StringComparer.Ordinal);
        return string.Join("|", entries);
    }
}
