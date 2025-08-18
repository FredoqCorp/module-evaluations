using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Context;

/// <summary>
/// Extensions to configure EF Core with Npgsql/PostgreSQL best practices for this module.
/// </summary>
public static class DbContextOptionsExtensions
{
    /// <summary>
    /// Registers DatabaseContext with PostgreSQL provider and recommended options.
    /// </summary>
    public static IServiceCollection AddEvaluationsDbContext(
        this IServiceCollection services,
        string connectionString,
        Action<NpgsqlDataSourceBuilder>? dataSourceConfigure = null)
    {
        services.AddDbContextFactory<DatabaseContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsql =>
            {
                npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                npgsql.EnableRetryOnFailure();
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", schema: "evaluations");
                npgsql.MapEnum<FormStatus>("form_status", "evaluations");
                npgsql.MapEnum<OptimizationGoal>("optimization_goal", "evaluations");
                npgsql.MapEnum<FormCalculationKind>("form_calculation_kind", "evaluations");
            });

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}
