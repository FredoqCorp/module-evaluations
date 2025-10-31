using System.Security.Cryptography;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Groups;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration coverage for PgWeightedGroups persistence logic.
/// </summary>
public sealed class PgWeightedGroupsTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    /// <summary>
    /// Builds the test suite with a shared database fixture.
    /// </summary>
    /// <param name="fixture">Shared PostgreSQL container fixture.</param>
    public PgWeightedGroupsTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    /// <inheritdoc />
    [Fact(DisplayName = "PgWeightedGroups adds weighted group to form")]
    public async Task PgWeightedGroups_adds_weighted_group_to_form()
    {
        var formId = new FormId(Guid.NewGuid());
        var profile = new GroupProfile(new GroupId(Guid.NewGuid()), new GroupTitle($"Название-{Guid.NewGuid():N}µ"), new GroupDescription($"Описание-{Guid.NewGuid():N}ñ"));
        var orderIndex = new OrderIndex(RandomNumberGenerator.GetInt32(1, int.MaxValue));
        var basis = (ushort)RandomNumberGenerator.GetInt32(1, 10001);
        var weight = new Weight(new BasisPoints(basis));
        var issuedAt = DateTimeOffset.UtcNow;

        await using (var preparation = new PostgresUnitOfWork(_fixture.ConnectionString))
        {
            var connection = await preparation.ActiveConnection();
            await connection.ExecuteAsync(
                "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
                new
                {
                    Id = formId.Value,
                    Name = $"Форма-{Guid.NewGuid():N}š",
                    Description = $"Текст-{Guid.NewGuid():N}đ",
                    Code = $"CODE-{RandomNumberGenerator.GetInt32(200, 9999)}",
                    Tags = "[]",
                    RootGroupType = "weighted",
                    CreatedAt = issuedAt,
                    UpdatedAt = issuedAt
                });
        }

        await using (var unitOfWork = new PostgresUnitOfWork(_fixture.ConnectionString))
        {
            var adapter = new PgWeightedGroups(unitOfWork);
            await adapter.Add(profile, formId, weight, orderIndex);
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
            "weighted",
            basis,
            orderIndex.Value,
            profile.Title.Text,
            profile.Description.Text);

        actual.ShouldBe(expected, "database snapshot is not aligned with expectations");
    }

    /// <inheritdoc />
    [Fact(DisplayName = "PgWeightedGroups rejects group when parent form is missing")]
    public async Task PgWeightedGroups_rejects_group_when_parent_form_is_missing()
    {
        var profile = new GroupProfile(new GroupId(Guid.NewGuid()), new GroupTitle($"Название-{Guid.NewGuid():N}æ"), new GroupDescription($"Описание-{Guid.NewGuid():N}ø"));
        var formId = new FormId(Guid.NewGuid());
        var orderIndex = new OrderIndex(RandomNumberGenerator.GetInt32(1, int.MaxValue));
        var weight = new Weight(new BasisPoints((ushort)RandomNumberGenerator.GetInt32(1, 10001)));

        await using var unitOfWork = new PostgresUnitOfWork(_fixture.ConnectionString);
        var adapter = new PgWeightedGroups(unitOfWork);

        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await adapter.Add(profile, formId, weight, orderIndex);
        });
    }

    /// <inheritdoc />
    [Fact(DisplayName = "PgWeightedGroups rejects group when weight violates constraints")]
    public async Task PgWeightedGroups_rejects_group_when_weight_violates_constraints()
    {
        var formId = new FormId(Guid.NewGuid());
        var profile = new GroupProfile(new GroupId(Guid.NewGuid()), new GroupTitle($"Название-{Guid.NewGuid():N}ù"), new GroupDescription($"Описание-{Guid.NewGuid():N}ŧ"));
        var orderIndex = new OrderIndex(RandomNumberGenerator.GetInt32(1, int.MaxValue));
        var issuedAt = DateTimeOffset.UtcNow;

        await using (var preparation = new PostgresUnitOfWork(_fixture.ConnectionString))
        {
            var connection = await preparation.ActiveConnection();
            await connection.ExecuteAsync(
                "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
                new
                {
                    Id = formId.Value,
                    Name = $"Форма-{Guid.NewGuid():N}ž",
                    Description = $"Текст-{Guid.NewGuid():N}ÿ",
                    Code = $"CODE-{RandomNumberGenerator.GetInt32(10000, 19999)}",
                    Tags = "[]",
                    RootGroupType = "weighted",
                    CreatedAt = issuedAt,
                    UpdatedAt = issuedAt
                });
        }

        await using var unitOfWork = new PostgresUnitOfWork(_fixture.ConnectionString);
        var adapter = new PgWeightedGroups(unitOfWork);
        var weight = new OverflowWeight();

        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await adapter.Add(profile, formId, weight, orderIndex);
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

    /// <summary>
    /// Test double that produces an overflow of basis points to trigger database constraints.
    /// </summary>
    private sealed class OverflowWeight : IWeight
    {
        private readonly IPercent _percent = new OverflowPercent();

        public IPercent Percent()
        {
            return _percent;
        }

        public CriterionScore Weighted(CriterionScore score)
        {
            return new CriterionScore(score.Value);
        }

        private sealed class OverflowPercent : IPercent
        {
            private readonly IBasisPoints _points = new OverflowBasisPoints();

            public IBasisPoints Basis()
            {
                return _points;
            }
        }

        private sealed class OverflowBasisPoints : IBasisPoints
        {
            public IPercent Percent()
            {
                return new OverflowPercent();
            }

            public decimal Apply(decimal value)
            {
                return 12000m;
            }
        }
    }
}
