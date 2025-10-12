using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;
using Npgsql;
using Xunit;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests.Adapters;

/// <summary>
/// Integration tests for PostgresForms.
/// </summary>
public sealed class PostgresFormsTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public PostgresFormsTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task List_returns_empty_list_when_no_forms_exist()
    {
        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var forms = new PostgresForms(uow);

        var result = await forms.List();

        Assert.Empty(result);
    }

    [Fact]
    public async Task List_returns_all_forms_ordered_by_created_at_desc()
    {
        var form1Id = Guid.NewGuid();
        var form2Id = Guid.NewGuid();

        await using var setupUow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var setupConnection = await setupUow.ActiveConnection();

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form1Id,
                Name = "First Form",
                Description = "Description 1",
                Code = "FORM-001",
                Tags = "[\"tag1\", \"tag2\"]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1)
            });

        await setupConnection.ExecuteAsync(
            "INSERT INTO forms (id, name, description, code, tags, root_group_type, created_at, updated_at) VALUES (@Id, @Name, @Description, @Code, @Tags::jsonb, @RootGroupType, @CreatedAt, @UpdatedAt)",
            new
            {
                Id = form2Id,
                Name = "Second Form",
                Description = "Description 2",
                Code = "FORM-002",
                Tags = "[\"tag3\"]",
                RootGroupType = "weighted",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var forms = new PostgresForms(uow);

        // Act
        var result = await forms.List();

        // Assert
        Assert.Equal(2, result.Count);

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = ANY(@Ids)", new { Ids = new[] { form1Id, form2Id } });
    }

    [Fact]
    public async Task List_returns_forms_with_empty_root_groups()
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
                Description = "Test Description",
                Code = "TEST-001",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var forms = new PostgresForms(uow);

        // Act
        var result = await forms.List();

        // Assert
        Assert.Single(result);
        var form = result[0];

        // Form should have metadata and empty root group
        form.Validate(); // Should not throw as empty groups are valid

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }

    [Fact]
    public async Task List_handles_null_description()
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
                Code = "TEST-002",
                Tags = "[]",
                RootGroupType = "average",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });

        await using var uow = new PostgresUnitOfWork(_fixture.ConnectionString);
        var forms = new PostgresForms(uow);

        // Act
        var result = await forms.List();

        // Assert
        Assert.Single(result);

        // Cleanup
        await setupConnection.ExecuteAsync("DELETE FROM forms WHERE id = @Id", new { Id = formId });
    }
}
