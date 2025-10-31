using System.Text.Json;
using CascVel.Modules.Evaluations.Management.Application.Ports;
using CascVel.Modules.Evaluations.Management.Application.UseCases.ListForms;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Infrastructure.Media;
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
            .Produces<ListFormsResponse>(StatusCodes.Status200OK, "application/json")
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

    private static async Task<Results<Ok<ListFormsResponse>, ProblemHttpResult>> ListFormsEndpoint(
        IListForms useCase,
        CancellationToken ct)
    {
        var response = await useCase.Execute(ct);

        return TypedResults.Ok(response);
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
    ///   "root": {
    ///     "criteria": [
    ///       {
    ///         "title": "Empathy",
    ///         "text": "Сотрудник проявил эмпатию",
    ///         "order": 0,
    ///         "weight": 60,
    ///         "ratingOptions": [
    ///           { "order": 0, "score": 5, "label": "High", "annotation": "Δ" }
    ///         ]
    ///       }
    ///     ],
    ///     "groups": [
    ///       {
    ///         "title": "Resolution",
    ///         "description": "Скорость реакции",
    ///         "order": 0,
    ///         "weight": 40,
    ///         "criteria": [],
    ///         "groups": []
    ///       }
    ///     ]
    ///   }
    /// }
    /// </summary>
    private static async Task<Results<FormCreatedContentResult, ProblemHttpResult>> PostFormEndpoint(
        IForms forms,
        HttpRequest request,
        CancellationToken ct)
    {
        try
        {
            using var document = await JsonDocument.ParseAsync(request.Body, cancellationToken: ct);
            var form = new JsonForm(document);
            var created = await forms.Add(form, ct);
            var id = form.Identity().Value;
            return Create(created, id);
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

    /// <summary>
    /// Builds the HTTP result that returns the created form JSON payload.
    /// </summary>
    /// <param name="form">Persisted form aggregate.</param>
    /// <param name="id">Unique identifier of the created form.</param>
    /// <returns>HTTP result that writes JSON payload.</returns>
    private static FormCreatedContentResult Create(IForm form, Guid id)
    {
        ArgumentNullException.ThrowIfNull(form);

        using var media = new JsonMediaWriter();
        form.Print(media);
        var payload = media.Output();
        return new FormCreatedContentResult(id, payload);
    }

    /// <summary>
    /// HTTP 201 result that writes the created form JSON representation.
    /// </summary>
    private sealed class FormCreatedContentResult : IResult
    {
        private readonly Guid _id;
        private readonly string _payload;

        /// <summary>
        /// Initializes the result with identifier and serialized payload.
        /// </summary>
        /// <param name="id">Identifier of the created form.</param>
        /// <param name="payload">Serialized JSON payload.</param>
        public FormCreatedContentResult(Guid id, string payload)
        {
            _id = id;
            _payload = payload;
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            httpContext.Response.StatusCode = StatusCodes.Status201Created;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.Headers.Location = $"/forms/{_id}";
            await httpContext.Response.WriteAsync(_payload);
        }
    }
}
