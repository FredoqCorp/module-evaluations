using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Domain.Models.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration tests validating PgForms.Form behavior.
/// </summary>
public sealed class PgFormsFormTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    /// <summary>
    /// Initializes the test fixture with shared database resources.
    /// </summary>
    /// <param name="fixture">Database fixture providing PostgreSQL access.</param>
    public PgFormsFormTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>
    /// Ensures that IForms.Form prints stored metadata and calculation type.
    /// </summary>
    [Fact]
    public async Task IForms_Form_prints_metadata_and_calculation()
    {
        await _fixture.Reset();
        var formId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var criterionId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        var code = $"code-{Guid.NewGuid():N}";
        var name = $"Форма-{Guid.NewGuid():N}";
        var description = $"Описание-{Guid.NewGuid():N}";
        var tags = JsonSerializer.Serialize(new[]
        {
            $"тег-{Guid.NewGuid():N}",
            $"标记-{Guid.NewGuid():N}"
        });
        var ratingLabel = $"Λейбл-{Guid.NewGuid():N}";
        var ratingAnnotation = $"注-{Guid.NewGuid():N}";
        var ratingOptions = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            ["0"] = new
            {
                score = RandomNumberGenerator.GetInt32(1, 6),
                label = ratingLabel,
                annotation = ratingAnnotation
            }
        });

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = name,
                Description = description,
                Code = code,
                Tags = tags,
                RootGroupType = "average",
                CreatedAt = now,
                UpdatedAt = now
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @WeightBasisPoints, @OrderIndex, @CreatedAt)",
            new
            {
                Id = groupId,
                FormId = formId,
                ParentId = (Guid?)null,
                Title = $"Группа-{Guid.NewGuid():N}",
                Description = $"Описание-{Guid.NewGuid():N}",
                WeightBasisPoints = (int?)null,
                OrderIndex = 0,
                CreatedAt = now
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterionId,
                FormId = formId,
                GroupId = groupId,
                Title = $"Критерий-{Guid.NewGuid():N}",
                Text = $"Описание-{Guid.NewGuid():N}",
                WeightBasisPoints = (int?)null,
                RatingOptions = ratingOptions,
                OrderIndex = 0,
                CreatedAt = now
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var adapter = new PgForms(uow);
        var target = await adapter.Form(new FormId(formId));
        using var media = new FakeMedia();

        await target.Print(media);

        var actualId = media.GetValue<Guid>("formId");
        var actualCalculation = media.GetValue<string>("calculation");

        Assert.Equal((formId, "average"), (actualId, actualCalculation));
    }
}
