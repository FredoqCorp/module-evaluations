using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Identity;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters;
using CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Groups;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using CascVel.Modules.Evaluations.Management.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CascVel.Modules.Evaluations.Management.Infrastructure;

/// <summary>
/// Dependency injection extensions for Infrastructure layer.
/// </summary>
public static class InfrastructureExtensions
{
    /// <summary>
    /// Registers Infrastructure services into the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="connectionString">PostgreSQL connection string.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        // Register UnitOfWork as scoped (one per request/scope)
        services.AddScoped<IUnitOfWork>(sp => new PostgresUnitOfWork(connectionString));

        // Register adapters
        services.AddScoped<IForms, PgForms>();
        services.AddScoped<IAverageGroups, PgAverageGroups>();
        services.AddScoped<IWeightedGroups, PgWeightedGroups>();

        // Register identity services
        services.AddScoped<IModuleUser, HttpContextModuleUser>();

        return services;
    }
}
