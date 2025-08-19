using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Context;

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
        dataSourceBuilder.MapEnum<OptimizationGoal>("evaluations.optimization_goal");
        dataSourceBuilder.MapEnum<FormCalculationKind>("evaluations.form_calculation_kind");
        var dataSource = dataSourceBuilder.Build();

        DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(dataSource, npgsql =>
            {
                // Ensure EF migrations history is stored in the same module schema
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "evaluations");
                npgsql.MapEnum<FormStatus>("form_status", "evaluations");
                npgsql.MapEnum<OptimizationGoal>("optimization_goal", "evaluations");
                npgsql.MapEnum<FormCalculationKind>("form_calculation_kind", "evaluations");
            });

        return new DatabaseContext(optionsBuilder.Options);
    }
}
