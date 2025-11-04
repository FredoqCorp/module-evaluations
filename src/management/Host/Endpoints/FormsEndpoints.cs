using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Host.Infrastructure;
using CascVel.Modules.Evaluations.Management.Host.Models.Forms;
using CascVel.Modules.Evaluations.Management.Host.Responses;
using Microsoft.AspNetCore.Http;
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
            .RequireAuthorization("FormDesigner")
            .Produces<IFormSummaries>(StatusCodes.Status200OK, "application/json")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        group.MapPost("/", PostFormEndpoint)
            .WithName("PostForm")
            .WithSummary("Create a new evaluation form")
            .WithDescription("Creates a new evaluation form; body must contain metadata, calculation rule, groups, and criteria")
            .RequireAuthorization("FormDesigner")
            .Accepts<JsonElement>("application/json")
            .Produces<IForm>(StatusCodes.Status201Created, "application/json")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> ListFormsEndpoint(
        IForms forms,
        HttpResponse response,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(forms);
        ArgumentNullException.ThrowIfNull(response);

        var summaries = await forms.List(ct);

        using var media = new FormSummariesResponseMedia(response);
        summaries.Print(media);
        return media.Output();
    }

    /// <summary>
    /// Expected JSON body (example):
    /// {
    ///   "metadata": {
    ///     "name": "Customer Satisfaction",
    ///     "description": "Post-support survey",
    ///     "code": "csat-ops",
    ///     "tags": ["voice", "β-test"]
    ///   },
    ///   "calculation": "weighted",
    ///   "criteria": [
    ///       {
    ///         "title": "Empathy",
    ///         "text": "Сотрудник проявил эмпатию",
    ///         "order": 0,
    ///         "weight": 60,
    ///         "ratingOptions": [
    ///           { "order": 0, "score": 5, "label": "High", "annotation": "Δ" }
    ///         ]
    ///       }
    ///   ],
    ///   "groups": [
    ///       {
    ///         "title": "Resolution",
    ///         "description": "Скорость реакции",
    ///         "order": 0,
    ///         "weight": 40,
    ///         "criteria": [],
    ///         "groups": []
    ///       }
    ///   ]
    /// }
    /// </summary>
    private static async Task<IResult> PostFormEndpoint(
        IForms forms,
        HttpRequest request,
        CancellationToken ct)
    {
        try
        {
            using var document = await JsonDocument.ParseAsync(request.Body, cancellationToken: ct);
            var form = new JsonNewForm(document);
            var created = await forms.Add(form, ct);
            using var media = new FormCreatedResponseMedia(request.HttpContext.Response);
            created.Print(media);
            return media.Output();
        }
        catch (JsonException ex)
        {
            return Failure(StatusCodes.Status400BadRequest, "Malformed form payload", ex.Message);
        }
        catch (ArgumentException ex)
        {
            return Failure(StatusCodes.Status400BadRequest, "Form validation failed", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Failure(StatusCodes.Status400BadRequest, "Form validation failed", ex.Message);
        }
    }

    private static ProblemHttpResult Failure(int status, string title, string detail)
    {
        return TypedResults.Problem(statusCode: status, title: title, detail: detail);
    }
}
