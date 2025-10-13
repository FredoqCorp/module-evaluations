using CascVel.Modules.Evaluations.Management.Application;
using CascVel.Modules.Evaluations.Management.Host.Endpoints;
using CascVel.Modules.Evaluations.Management.Host.Infrastructure;
using CascVel.Modules.Evaluations.Management.Infrastructure;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();
builder.Services.AddHealthChecks();

// Configure ProblemDetails support
builder.Services.AddProblemDetails();

// Add global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Configure OpenAPI with schema transformer for Printer Pattern types
builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer<ListFormsResponseSchemaTransformer>();
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new FormSummaryJsonConverter());
});

WebApplication app = builder.Build();

// Enable exception handling middleware
app.UseExceptionHandler();

// Enable status code pages for proper ProblemDetails responses
app.UseStatusCodePages();

app.MapHealthChecks("/health");
app.MapGet("/ping", () => Results.Ok(new { ok = true, ts = DateTimeOffset.UtcNow }));

app.MapOpenApi();
app.MapScalarApiReference();

app.MapFormsEndpoints();

await app.RunAsync();


/// <summary>
/// Entry point for the application. Made accessible to WebApplicationFactory for E2E tests.
/// </summary>
public partial class Program
{
    /// <summary>
    /// Required for WebApplicationFactory.
    /// </summary>
    protected Program() { }
}
