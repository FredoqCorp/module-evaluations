using System.Security.Cryptography;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Groups;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration coverage for PgAverageGroups persistence logic.
/// </summary>
public sealed class PgAverageGroupsTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    /// <summary>
    /// Builds the test suite with a shared database fixture.
    /// </summary>
    /// <param name="fixture">Shared PostgreSQL container fixture.</param>
    public PgAverageGroupsTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    /// <inheritdoc />
    [Fact(DisplayName = "PgAverageGroups adds average group to form")]
    public async Task PgAverageGroups_adds_average_group_to_form()
    {
        var formId = new FormId(Guid.NewGuid());
        var profile = new GroupProfile(new GroupId(Guid.NewGuid()), new GroupTitle($"Название-{Guid.NewGuid():N}δ"), new GroupDescription($"Описание-{Guid.NewGuid():N}φ"));
        var index = new OrderIndex(RandomNumberGenerator.GetInt32(0, int.MaxValue));
        var issuedAt = DateTimeOffset.UtcNow;

        await using (var preparation = new PostgresUnitOfWork(_fixture.ConnectionString))
        {
            var connection = await preparation.ActiveConnection();
            await connection.ExecuteAsync(
                "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
                new
                {
                    Id = formId.Value,
                    Name = $"Форма-{Guid.NewGuid():N}π",
                    Description = $"Текст-{Guid.NewGuid():N}λ",
                    Code = $"CODE-{RandomNumberGenerator.GetInt32(100, 999)}",
                    Tags = "[]",
                    RootGroupType = "average",
                    CreatedAt = issuedAt,
                    UpdatedAt = issuedAt
                });
        }

        await using (var unitOfWork = new PostgresUnitOfWork(_fixture.ConnectionString))
        {
            var adapter = new PgAverageGroups(unitOfWork);
            await adapter.Add(profile, formId, index);
        }

        await using var verifier = new PostgresUnitOfWork(_fixture.ConnectionString);
        var connectionView = await verifier.ActiveConnection();
        var row = await connectionView.QuerySingleAsync<(Guid Id, Guid FormId, Guid? ParentId, string Type, int? Weight, int OrderIndex, string Title, string Description)>(
            "SELECT id, form_id, parent_id, group_type, weight_basis_points, order_index, title, description FROM form_groups WHERE id = @Id",
            new { Id = profile.Id.Value });
        var actual = new GroupSnapshot(
            row.Id,
            row.FormId,
            row.ParentId,
            row.Type,
            row.Weight,
            row.OrderIndex,
            row.Title,
            row.Description);
        var expected = new GroupSnapshot(
            profile.Id.Value,
            formId.Value,
            null,
            "average",
            null,
            index.Value,
            profile.Title.Text,
            profile.Description.Text);

        actual.ShouldBe(expected, "database snapshot is not aligned with expectations");
    }

    /// <inheritdoc />
    [Fact(DisplayName = "PgAverageGroups rejects group when parent is missing")]
    public async Task PgAverageGroups_rejects_group_when_parent_is_missing()
    {
        var profile = new GroupProfile(new GroupId(Guid.NewGuid()), new GroupTitle($"Название-{Guid.NewGuid():N}ψ"), new GroupDescription($"Описание-{Guid.NewGuid():N}ω"));
        var formId = new FormId(Guid.NewGuid());
        var index = new OrderIndex(RandomNumberGenerator.GetInt32(1, int.MaxValue));

        await using var unitOfWork = new PostgresUnitOfWork(_fixture.ConnectionString);
        var adapter = new PgAverageGroups(unitOfWork);

        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await adapter.Add(profile, formId, index);
        });
    }

    private sealed record GroupSnapshot(
        Guid Id,
        Guid FormId,
        Guid? ParentId,
        string Type,
        int? Weight,
        int OrderIndex,
        string Title,
        string Description);
}
