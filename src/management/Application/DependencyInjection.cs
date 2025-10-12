using CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;
using Microsoft.Extensions.DependencyInjection;

namespace CascVel.Modules.Evaluations.Management.Application;

/// <summary>
/// Dependency injection extensions for Application layer.
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Registers Application services into the DI container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IListForms, ListForms>();

        return services;
    }
}
