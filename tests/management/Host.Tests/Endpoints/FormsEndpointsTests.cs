using System.Net;
using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Host.Tests.Infrastructure;
using Dapper;
using Npgsql;
using Shouldly;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Host.Tests.Endpoints;

/// <summary>
/// E2E tests for /api/forms endpoints.
/// </summary>
public sealed class FormsEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public FormsEndpointsTests(TestWebApplicationFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_api_forms_returns_empty_list_when_no_forms_exist()
    {
        // Act
        var response = await _client.GetAsync(new Uri("/forms", UriKind.Relative));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        document.RootElement.GetProperty("forms").GetArrayLength().ShouldBe(0);
    }

    [Fact]
    public async Task GET_api_forms_returns_single_form_with_correct_structure()
    {
        // Arrange
        var formId = Guid.NewGuid();
        await using var connection = new NpgsqlConnection(_factory.ConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Test Form",
                Description = "Test Description",
                Code = "TEST-001",
                Tags = "[\"test\", \"e2e\"]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        // Act
        var response = await _client.GetAsync(new Uri("/forms", UriKind.Relative));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        document.RootElement.GetProperty("forms").GetArrayLength().ShouldBe(1);

        var form = document.RootElement.GetProperty("forms")[0];
        form.GetProperty("id").GetGuid().ShouldBe(formId);
        form.GetProperty("name").GetString().ShouldBe("Test Form");
        form.GetProperty("description").GetString().ShouldBe("Test Description");
        form.GetProperty("code").GetString().ShouldBe("TEST-001");
        form.GetProperty("tags").GetArrayLength().ShouldBe(2);
        form.GetProperty("tags")[0].GetString().ShouldBe("test");
        form.GetProperty("tags")[1].GetString().ShouldBe("e2e");
        form.GetProperty("groupsCount").GetInt32().ShouldBe(0);
        form.GetProperty("criteriaCount").GetInt32().ShouldBe(0);
        form.GetProperty("calculationType").GetString().ShouldBe("Average");

        // Cleanup
        await connection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task GET_api_forms_returns_form_with_groups_and_criteria_counts()
    {
        // Arrange
        var formId = Guid.NewGuid();
        var group1Id = Guid.NewGuid();
        var group2Id = Guid.NewGuid();
        var criterion1Id = Guid.NewGuid();
        var criterion2Id = Guid.NewGuid();
        var criterion3Id = Guid.NewGuid();

        await using var connection = new NpgsqlConnection(_factory.ConnectionString);
        await connection.OpenAsync();

        // Insert form
        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Complex Form",
                Description = "Form with groups and criteria",
                Code = "COMPLEX-001",
                Tags = "[]",
                RootGroupType = "weighted",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        // Insert groups
        await connection.ExecuteAsync(
            "INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @WeightBasisPoints, @OrderIndex, @CreatedAt)",
            new
            {
                Id = group1Id,
                FormId = formId,
                ParentId = (Guid?)null,
                Title = "Group 1",
                Description = "First group",
                WeightBasisPoints = 6000,
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await connection.ExecuteAsync(
            "INSERT INTO form_groups (id, form_id, parent_id, title, description, weight_basis_points, order_index, created_at) VALUES (@Id, @FormId, @ParentId, @Title, @Description, @WeightBasisPoints, @OrderIndex, @CreatedAt)",
            new
            {
                Id = group2Id,
                FormId = formId,
                ParentId = (Guid?)null,
                Title = "Group 2",
                Description = "Second group",
                WeightBasisPoints = 4000,
                OrderIndex = 2,
                CreatedAt = DateTimeOffset.UtcNow
            });

        // Insert criteria
        await connection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion1Id,
                FormId = formId,
                GroupId = group1Id,
                Title = "Criterion 1",
                Text = "First criterion",
                WeightBasisPoints = 5000,
                RatingOptions = "{\"0\":{\"score\":5,\"label\":\"Excellent\",\"annotation\":\"\"},\"1\":{\"score\":3\",\"label\":\"Good\",\"annotation\":\"\"},\"2\":{\"score\":1,\"label\":\"Poor\",\"annotation\":\"\"}}",
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await connection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion2Id,
                FormId = formId,
                GroupId = group1Id,
                Title = "Criterion 2",
                Text = "Second criterion",
                WeightBasisPoints = 5000,
                RatingOptions = "{\"0\":{\"score\":5,\"label\":\"Excellent\",\"annotation\":\"\"},\"1\":{\"score\":3,\"label\":\"Good\",\"annotation\":\"\"},\"2\":{\"score\":1\",\"label\":\"Poor\",\"annotation\":\"\"}}",
                OrderIndex = 2,
                CreatedAt = DateTimeOffset.UtcNow
            });

        await connection.ExecuteAsync(
            "INSERT INTO form_criteria (id, form_id, group_id, title, text, weight_basis_points, rating_options, order_index, created_at) VALUES (@Id, @FormId, @GroupId, @Title, @Text, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)",
            new
            {
                Id = criterion3Id,
                FormId = formId,
                GroupId = group2Id,
                Title = "Criterion 3",
                Text = "Third criterion",
                WeightBasisPoints = 10000,
                RatingOptions = "{\"0\":{\"score\":5,\"label\":\"Excellent\",\"annotation\":\"\"},\"1\":{\"score\":3,\"label\":\"Good\",\"annotation\":\"\"},\"2\":{\"score\":1,\"label\":\"Poor\",\"annotation\":\"\"}}",
                OrderIndex = 1,
                CreatedAt = DateTimeOffset.UtcNow
            });

        // Act
        var response = await _client.GetAsync(new Uri("/forms", UriKind.Relative));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        document.RootElement.GetProperty("forms").GetArrayLength().ShouldBe(1);

        var form = document.RootElement.GetProperty("forms")[0];
        form.GetProperty("id").GetGuid().ShouldBe(formId);
        form.GetProperty("groupsCount").GetInt32().ShouldBe(2);
        form.GetProperty("criteriaCount").GetInt32().ShouldBe(3);
        form.GetProperty("calculationType").GetString().ShouldBe("WeightedAverage");

        // Cleanup
        await connection.ExecuteAsync("DELETE FROM form_criteria WHERE form_id = @Id", new { Id = formId });
        await connection.ExecuteAsync("DELETE FROM form_groups WHERE form_id = @Id", new { Id = formId });
        await connection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task GET_api_forms_returns_multiple_forms_ordered_by_created_at_desc()
    {
        // Arrange
        var form1Id = Guid.NewGuid();
        var form2Id = Guid.NewGuid();
        var form3Id = Guid.NewGuid();

        await using var connection = new NpgsqlConnection(_factory.ConnectionString);
        await connection.OpenAsync();

        var now = DateTimeOffset.UtcNow;

        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form1Id,
                Name = "First Form",
                Description = "Created first",
                Code = "FORM-001",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now.AddDays(-2)
            });

        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form2Id,
                Name = "Second Form",
                Description = "Created second",
                Code = "FORM-002",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = now,
                UpdatedAt = now
            });

        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form3Id,
                Name = "Third Form",
                Description = "Created third",
                Code = "FORM-003",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = now.AddDays(-1),
                UpdatedAt = now.AddDays(-1)
            });

        // Act
        var response = await _client.GetAsync(new Uri("/forms", UriKind.Relative));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        // Find our test forms in the response
        var forms = document.RootElement.GetProperty("forms").EnumerateArray()
            .Where(f => new[] { form1Id, form2Id, form3Id }.Contains(f.GetProperty("id").GetGuid()))
            .ToList();

        forms.Count.ShouldBe(3);

        // Most recent should be first (ordered by created_at desc)
        forms[0].GetProperty("id").GetGuid().ShouldBe(form2Id); // now
        forms[1].GetProperty("id").GetGuid().ShouldBe(form3Id); // now - 1 day
        forms[2].GetProperty("id").GetGuid().ShouldBe(form1Id); // now - 2 days

        // Cleanup
        await connection.ExecuteAsync("DELETE FROM forms WHERE id = ANY(@Ids)",
            new { Ids = new[] { form1Id, form2Id, form3Id } });
    }

    [Fact]
    public async Task GET_api_forms_handles_null_description()
    {
        // Arrange
        var formId = Guid.NewGuid();
        await using var connection = new NpgsqlConnection(_factory.ConnectionString);
        await connection.OpenAsync();

        await connection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = formId,
                Name = "Form Without Description",
                Description = (string?)null,
                Code = "NO-DESC",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        // Act
        var response = await _client.GetAsync(new Uri("/forms", UriKind.Relative));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var json = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(json);

        var forms = document.RootElement.GetProperty("forms");
        var form = forms.EnumerateArray().First(f => f.GetProperty("id").GetGuid() == formId);

        form.GetProperty("description").GetString().ShouldBe(string.Empty);

        // Cleanup
        await connection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task GET_api_forms_returns_correct_content_type_header()
    {
        // Act
        var response = await _client.GetAsync(new Uri("/forms", UriKind.Relative));

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.ShouldBe("application/json");
        response.Content.Headers.ContentType?.CharSet.ShouldBe("utf-8");
    }
}
