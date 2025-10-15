using CascVel.Modules.Evaluations.Management.Application;
using CascVel.Modules.Evaluations.Management.Domain.Enums;
using CascVel.Modules.Evaluations.Management.Host.Endpoints;
using CascVel.Modules.Evaluations.Management.Host.Infrastructure;
using CascVel.Modules.Evaluations.Management.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();

// Configure JWT Bearer authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Configuration.GetSection("JwtBearer");

        options.Authority = jwtConfig["Authority"];
        options.Audience = jwtConfig["Audience"];
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    });

// Configure authorization policies for module roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("FormDesigner", policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(context =>
                  context.User.HasClaim("module_role", ModuleRole.FormDesigner.ToString())));

    options.AddPolicy("Supervisor", policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(context =>
                  context.User.HasClaim("module_role", ModuleRole.Supervisor.ToString())));

    options.AddPolicy("Operator", policy =>
        policy.RequireAuthenticatedUser()
              .RequireAssertion(context =>
                  context.User.HasClaim("module_role", ModuleRole.Operator.ToString())));
});

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

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

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
