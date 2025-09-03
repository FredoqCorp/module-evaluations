using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// Design-time factory for EF Core migrations/tools.
/// Keeps Infrastructure migrations independent from Host startup.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    /// <summary>
    /// Creates DatabaseContext for design-time operations (migrations).
    /// </summary>
    /// <param name="args">CLI arguments.</param>
    /// <returns>Configured <see cref="DatabaseContext"/>.</returns>
    public DatabaseContext CreateDbContext(string[] args)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? Environment.GetEnvironmentVariable("EVALUATIONS_DB_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=module_evaluations;Username=postgres;Password=postgres";

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        dataSourceBuilder.MapEnum<FormStatus>("evaluations.form_status");
        var dataSource = dataSourceBuilder.Build();

        DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(dataSource, npgsql =>
            {
                // Ensure EF migrations history is stored in the same module schema
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "evaluations");
                npgsql.MapEnum<FormStatus>("form_status", "evaluations");
            });

        return new DatabaseContext(optionsBuilder.Options);
    }
}
