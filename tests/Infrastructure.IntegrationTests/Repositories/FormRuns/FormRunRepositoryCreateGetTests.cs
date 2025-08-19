using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using CascVel.Module.Evaluations.Management.Infrastructure.Repositories;
using Infrastructure.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CascVel.Module.Evaluations.Management.Infrastructure.IntegrationTests.Repositories.FormRuns;

/// <summary>
/// Integration test that verifies create and get operations return the same aggregate root for FormRunRepository
/// Example: Create a run with non-ASCII values then load it by id and compare Meta.RunFor
/// </summary>
[Collection("postgres-db")]
public sealed class FormRunRepositoryCreateGetTests
{
    private readonly PostgresFixture _fx;

    /// <summary>
    /// Creates a new instance of the test using the shared Postgres fixture
    /// </summary>
    /// <param name="fx">The Postgres fixture that provides database services</param>
    public FormRunRepositoryCreateGetTests(PostgresFixture fx)
    {
        _fx = fx;
    }

    /// <summary>
    /// Verifies that creating and then retrieving a run preserves the RunFor value
    /// </summary>
    [Fact(DisplayName = "Create and get form run returns the same aggregate")]
    public async Task Create_and_get_form_run_returns_the_same_aggregate()
    {
        await _fx.ResetAsync();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        await using var scope = _fx.Services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FormRunRepository>>();
        var repo = new FormRunRepository(factory, logger);
        var now = DateTime.UtcNow;
        var run = new FormRun
        {
            Meta = new RunMeta
            {
                Form = new RunFormRef
                {
                    FormId = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, int.MaxValue),
                    FormCode = Guid.NewGuid().ToString("N"),
                },
                RunFor = $"r-Ω-{Guid.NewGuid():N}",
                SupervisorComment = "Комментарий ♣",
            },
            State = new RunState
            {
                Lifecycle = new RunLifecycle
                {
                    Launched = new Stamp { UserId = "u-σ", At = now },
                    FirstSaved = null,
                    LastSaved = null,
                    Published = null,
                },
                Context = new RunContext { Items = new Dictionary<string, string>(StringComparer.Ordinal) },
                Result = new RunResult { CurrentTotal = null, Criteria = [] },
                Agreement = null,
            },
        };
        long id = await repo.CreateAsync(run, cts.Token);
        var copy = await repo.GetAsync(id, isFullInclude: true, cts.Token);
        Assert.True(string.Equals(copy.Meta.RunFor, run.Meta.RunFor, StringComparison.Ordinal),
            "Loaded run RunFor is different which is a failure");
    }
}
