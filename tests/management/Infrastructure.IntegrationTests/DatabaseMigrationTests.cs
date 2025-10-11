using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.IntegrationTests;

/// <summary>
/// Integration tests for database migrations using DbUp.
/// </summary>
public sealed class DatabaseMigrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ITestOutputHelper _output;

    public DatabaseMigrationTests(DatabaseFixture fixture, ITestOutputHelper output)
    {
        _fixture = fixture;
        _output = output;
    }

    [Fact]
    public async Task Database_ShouldHaveAllTables()
    {
        // Arrange
        await using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();

        // Act
        await using var cmd = new NpgsqlCommand(@"
            SELECT table_name
            FROM information_schema.tables
            WHERE table_schema = 'public'
              AND table_name IN ('forms', 'form_groups', 'form_criteria', 'rating_options', 'schemaversions')
            ORDER BY table_name", connection);

        var tables = new List<string>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            tables.Add(reader.GetString(0));
        }

        // Assert
        Assert.Contains("forms", tables);
        Assert.Contains("form_groups", tables);
        Assert.Contains("form_criteria", tables);
        Assert.Contains("rating_options", tables);
        Assert.Contains("schemaversions", tables); // DbUp tracking table

        _output.WriteLine($"Found tables: {string.Join(", ", tables)}");
    }

    [Fact]
    public async Task DbUpTrackingTable_ShouldHaveExecutedScripts()
    {
        // Arrange
        await using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();

        // Act
        await using var cmd = new NpgsqlCommand(@"
            SELECT scriptname, applied
            FROM schemaversions
            ORDER BY schemaversionsid", connection);

        var scripts = new List<(string Name, DateTime Applied)>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            scripts.Add((reader.GetString(0), reader.GetDateTime(1)));
        }

        // Assert
        Assert.NotEmpty(scripts);
        Assert.Contains(scripts, s => s.Name.Contains("0001-initial-schema", StringComparison.Ordinal));
        Assert.Contains(scripts, s => s.Name.Contains("0002-add-type-constraints", StringComparison.Ordinal));

        foreach (var (name, applied) in scripts)
        {
            _output.WriteLine($"Script: {name}, Applied: {applied:yyyy-MM-dd HH:mm:ss}");
        }
    }

    [Fact]
    public Task Migrations_ShouldBeIdempotent()
    {
        // Arrange & Act - Run migrations again
        var result = DatabaseMigrator.MigrateDatabase(_fixture.ConnectionString);

        // Assert
        Assert.True(result.Successful);
        _output.WriteLine("Idempotency test passed - migrations can be run multiple times");

        return Task.CompletedTask;
    }

    [Fact]
    public void IsUpgradeRequired_ShouldReturnFalse_AfterInitialMigration()
    {
        // Act
        var isRequired = DatabaseMigrator.IsUpgradeRequired(_fixture.ConnectionString);

        // Assert
        Assert.False(isRequired);
        _output.WriteLine("No pending migrations - database is up to date");
    }

    [Fact]
    public async Task Forms_Table_ShouldHaveCorrectConstraints()
    {
        // Arrange
        await using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();

        // Act - Check constraints
        await using var cmd = new NpgsqlCommand(@"
            SELECT conname
            FROM pg_constraint
            WHERE conrelid = 'forms'::regclass
              AND contype = 'c'
            ORDER BY conname", connection);

        var constraints = new List<string>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            constraints.Add(reader.GetString(0));
        }

        // Assert
        Assert.Contains("chk_forms_valid_root_type", constraints);

        _output.WriteLine($"Found CHECK constraints: {string.Join(", ", constraints)}");
    }

    [Fact]
    public async Task FormGroups_Table_ShouldHaveForeignKeys()
    {
        // Arrange
        await using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();

        // Act - Check foreign keys
        await using var cmd = new NpgsqlCommand(@"
            SELECT conname
            FROM pg_constraint
            WHERE conrelid = 'form_groups'::regclass
              AND contype = 'f'
            ORDER BY conname", connection);

        var foreignKeys = new List<string>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            foreignKeys.Add(reader.GetString(0));
        }

        // Assert
        Assert.Contains("fk_form_groups_form_id", foreignKeys);
        Assert.Contains("fk_form_groups_parent_id", foreignKeys);

        _output.WriteLine($"Found foreign keys: {string.Join(", ", foreignKeys)}");
    }

    [Fact]
    public async Task AllTables_ShouldHaveIndexes()
    {
        // Arrange
        await using var connection = new NpgsqlConnection(_fixture.ConnectionString);
        await connection.OpenAsync();

        // Act
        await using var cmd = new NpgsqlCommand(@"
            SELECT tablename, indexname
            FROM pg_indexes
            WHERE schemaname = 'public'
              AND tablename LIKE 'form%' OR tablename = 'rating_options'
            ORDER BY tablename, indexname", connection);

        var indexes = new List<(string Table, string Index)>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            indexes.Add((reader.GetString(0), reader.GetString(1)));
        }

        // Assert
        Assert.NotEmpty(indexes);
        Assert.Contains(indexes, i => i.Index == "idx_forms_code");
        Assert.Contains(indexes, i => i.Index == "idx_forms_tags");
        Assert.Contains(indexes, i => i.Index == "idx_form_groups_form_id");

        foreach (var (table, index) in indexes)
        {
            _output.WriteLine($"Table: {table}, Index: {index}");
        }
    }
}
