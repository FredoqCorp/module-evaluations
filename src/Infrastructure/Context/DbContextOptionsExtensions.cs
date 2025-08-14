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
            });

            // Read-heavy default; override per query when needed
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
}
