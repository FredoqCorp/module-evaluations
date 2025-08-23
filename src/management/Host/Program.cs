WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


WebApplication app = builder.Build();


app.MapHealthChecks("/health");
app.MapGet("/ping", () => Results.Ok(new { ok = true, ts = DateTimeOffset.UtcNow }));

await app.RunAsync();
