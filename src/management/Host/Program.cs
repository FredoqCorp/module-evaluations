using CascVel.Modules.Evaluations.Management.Application;
using CascVel.Modules.Evaluations.Management.Host.Endpoints;
using CascVel.Modules.Evaluations.Management.Host.Infrastructure;
using CascVel.Modules.Evaluations.Management.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new FormSummaryJsonConverter());
});

WebApplication app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/ping", () => Results.Ok(new { ok = true, ts = DateTimeOffset.UtcNow }));

app.MapFormsEndpoints();

await app.RunAsync();
