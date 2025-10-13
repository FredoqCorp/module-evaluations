using CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CascVel.Modules.Evaluations.Management.Host.Endpoints;

/// <summary>
/// Defines HTTP endpoints for evaluation forms management.
/// </summary>
public static class FormsEndpoints
{
    /// <summary>
    /// Maps all forms-related endpoints to the route builder.
    /// </summary>
    /// <param name="app">Endpoint route builder for registering routes.</param>
    /// <returns>The same endpoint route builder for fluent chaining.</returns>
    public static IEndpointRouteBuilder MapFormsEndpoints(this IEndpointRouteBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var group = app.MapGroup("/forms")
            .WithTags("Forms");

        group.MapGet("/", ListFormsEndpoint)
            .WithName("ListForms")
            .WithSummary("Retrieve all evaluation forms")
            .WithDescription("Returns a list of all evaluation forms with their metadata and structural statistics")
            .Produces<ListFormsResponse>(StatusCodes.Status200OK, "application/json")
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<Results<Ok<ListFormsResponse>, ProblemHttpResult>> ListFormsEndpoint(
        IListForms useCase,
        CancellationToken ct)
    {
        var response = await useCase.Execute(ct);

        return TypedResults.Ok(response);
    }
}
