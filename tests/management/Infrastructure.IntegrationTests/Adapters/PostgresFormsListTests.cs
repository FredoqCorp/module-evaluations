using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.TestDoubles;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;
using Shouldly;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration tests for PostgresForms List method.
/// </summary>
public sealed class PostgresFormsListTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public PostgresFormsListTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task List_ReturnsEmptyList_WhenNoFormsExist()
    {
        // Arrange
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var forms = new PgForms(uow);

        // Act
        var result = await forms.List();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task List_ReturnsSingleForm_WithCorrectMetadataAndCounts()
    {
        // Arrange
        var formId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var criterion1Id = Guid.NewGuid();
        var criterion2Id = Guid.NewGuid();

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        // Insert form
        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Test Form",
                Description = "Test Description",
                Code = "TEST-001",
                Tags = "[\"tag1\", \"tag2\"]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        // Insert one group
        await setupConnection.ExecuteAsync(
            "INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @WeightBasisPoints, @OrderIndex, @CreatedAt)",
            new
            {
                Id = groupId,
                FormId = formId,
                ParentId = (Guid?)null,
                Title = "Test Group",
                Description = "Test group description",
                WeightBasisPoints = (int?)null,
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        // Insert two criteria
        await setupConnection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion1Id,
                FormId = formId,
                GroupId = groupId,
                Title = "Criterion 1",
                Text = "Test criterion 1 text",
                WeightBasisPoints = (int?)null,
                RatingOptions = "{\"0\":{\"score\":5,\"label\":\"High\",\"annotation\":\"\"}}",
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion2Id,
                FormId = formId,
                GroupId = groupId,
                Title = "Criterion 2",
                Text = "Test criterion 2 text",
                WeightBasisPoints = (int?)null,
                RatingOptions = "{\"0\":{\"score\":4,\"label\":\"Mid\",\"annotation\":\"\"}}",
                OrderIndex = 2,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var formsAdapter = new PgForms(uow);

        // Act
        var result = await formsAdapter.List();

        // Assert
        result.Count.ShouldBe(1);
        var summary = result[0];

        using var media = new FakeMedia();
        summary.Print(media);

        media.GetValue<Guid>("id").ShouldBe(formId);
        media.GetValue<string>("name").ShouldBe("Test Form");
        media.GetValue<string>("description").ShouldBe("Test Description");
        media.GetValue<string>("code").ShouldBe("TEST-001");

        var tags = media.GetValue<string[]>("tags");
        tags.ShouldNotBeNull();
        tags.Length.ShouldBe(2);
        tags[0].ShouldBe("tag1");
        tags[1].ShouldBe("tag2");

        media.GetValue<int>("groupsCount").ShouldBe(1);
        media.GetValue<int>("criteriaCount").ShouldBe(2);
        media.GetValue<string>("calculationType").ShouldBe("Average");

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM form_criteria WHERE form_id = @Id", new { Id = formId });
        await setupConnection.ExecuteAsync("DELETE FROM form_groups WHERE form_id = @Id", new { Id = formId });
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task List_ReturnsMultipleForms_OrderedByCreatedAtDesc()
    {
        // Arrange
        var form1Id = Guid.NewGuid();
        var form2Id = Guid.NewGuid();
        var form3Id = Guid.NewGuid();

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        var now = DateTimeOffset.UtcNow;

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form1Id,
                Name = "First Form",
                Description = "Description 1",
                Code = "FORM-001",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddDays(-2)
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form2Id,
                Name = "Second Form",
                Description = "Description 2",
                Code = "FORM-002",
                Tags = "[]",
                RootGroupType = "weighted",
                CreatedAt = now,
                UpdatedAt = now
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form3Id,
                Name = "Third Form",
                Description = "Description 3",
                Code = "FORM-003",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var formsAdapter = new PgForms(uow);

        // Act
        var result = await formsAdapter.List();

        // Assert
        result.Count.ShouldBeGreaterThanOrEqualTo(3);

        var indexById = new Dictionary<Guid, int>();
        for (var i = 0; i < result.Count; i++)
        {
            using var media = new FakeMedia();
            result[i].Print(media);
            var id = media.GetValue<Guid>("id");
            indexById[id] = i;
        }

        indexById.ShouldContainKey(form2Id);
        indexById.ShouldContainKey(form3Id);
        indexById.ShouldContainKey(form1Id);

        indexById[form2Id].ShouldBeLessThan(indexById[form3Id]);
        indexById[form3Id].ShouldBeLessThan(indexById[form1Id]);

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = ANY(@Ids)",
            new { Ids = new[] { form1Id, form2Id, form3Id } });
    }

    [Fact]
    public async Task List_HandlesNullDescription()
    {
        // Arrange
        var formId = Guid.NewGuid();

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Test Form",
                Description = (string?)null,
                Code = "TEST-NULL",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var formsAdapter = new PgForms(uow);

        // Act
        var result = await formsAdapter.List();

        // Assert
        result.Count.ShouldBeGreaterThanOrEqualTo(1);
        var summary = result.First(s =>
        {
            using var m = new FakeMedia();
            s.Print(m);
            return m.GetValue<Guid>("id") == formId;
        });

        using var media = new FakeMedia();
        summary.Print(media);
        media.GetValue<string>("description").ShouldBe(string.Empty);

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task List_ReturnsZeroCounts_WhenNoGroupsOrCriteria()
    {
        // Arrange
        var formId = Guid.NewGuid();

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Empty Form",
                Description = "No groups or criteria",
                Code = "EMPTY-001",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var formsAdapter = new PgForms(uow);

        // Act
        var result = await formsAdapter.List();

        // Assert
        result.Count.ShouldBeGreaterThanOrEqualTo(1);
        var summary = result.First(s =>
        {
            using var m = new FakeMedia();
            s.Print(m);
            return m.GetValue<Guid>("id") == formId;
        });

        using var media = new FakeMedia();
        summary.Print(media);
        media.GetValue<int>("groupsCount").ShouldBe(0);
        media.GetValue<int>("criteriaCount").ShouldBe(0);

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task List_CorrectlyCountsMultipleGroupsAndCriteria()
    {
        // Arrange
        var formId = Guid.NewGuid();
        var group1Id = Guid.NewGuid();
        var group2Id = Guid.NewGuid();
        var criterion1Id = Guid.NewGuid();
        var criterion2Id = Guid.NewGuid();
        var criterion3Id = Guid.NewGuid();

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        // Insert form
        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Complex Form",
                Description = "Multiple groups and criteria",
                Code = "COMPLEX-001",
                Tags = "[]",
                RootGroupType = "weighted",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        // Insert two groups
        await setupConnection.ExecuteAsync(
            "INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @WeightBasisPoints, @OrderIndex, @CreatedAt)",
            new
            {
                Id = group1Id,
                FormId = formId,
                ParentId = (Guid?)null,
                Title = "Group 1",
                Description = "Group 1 description",
                WeightBasisPoints = 6000,
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @WeightBasisPoints, @OrderIndex, @CreatedAt)",
            new
            {
                Id = group2Id,
                FormId = formId,
                ParentId = (Guid?)null,
                Title = "Group 2",
                Description = "Group 2 description",
                WeightBasisPoints = 4000,
                OrderIndex = 2,
                CreatedAt = DateTimeOffset.UtcNow
            });

        // Insert three criteria
        await setupConnection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion1Id,
                FormId = formId,
                GroupId = group1Id,
                Title = "Criterion 1",
                Text = "Test criterion 1 text",
                WeightBasisPoints = 2000,
                RatingOptions = "{\"0\":{\"score\":7,\"label\":\"Peak\",\"annotation\":\"\"}}",
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion2Id,
                FormId = formId,
                GroupId = group1Id,
                Title = "Criterion 2",
                Text = "Test criterion 2 text",
                WeightBasisPoints = 3000,
                RatingOptions = "{\"0\":{\"score\":6,\"label\":\"Solid\",\"annotation\":\"\"}}",
                OrderIndex = 2,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion3Id,
                FormId = formId,
                GroupId = group2Id,
                Title = "Criterion 3",
                Text = "Test criterion 3 text",
                WeightBasisPoints = 2000,
                RatingOptions = "{\"0\":{\"score\":8,\"label\":\"Sharp\",\"annotation\":\"\"}}",
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var formsAdapter = new PgForms(uow);

        // Act
        var result = await formsAdapter.List();

        // Assert
        result.Count.ShouldBeGreaterThanOrEqualTo(1);
        var summary = result.First(s =>
        {
            using var m = new FakeMedia();
            s.Print(m);
            return m.GetValue<Guid>("id") == formId;
        });

        using var media = new FakeMedia();
        summary.Print(media);
        media.GetValue<int>("groupsCount").ShouldBe(2);
        media.GetValue<int>("criteriaCount").ShouldBe(3);
        media.GetValue<string>("calculationType").ShouldBe("WeightedAverage");

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM form_criteria WHERE form_id = @Id", new { Id = formId });
        await setupConnection.ExecuteAsync("DELETE FROM form_groups WHERE form_id = @Id", new { Id = formId });
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }
}
