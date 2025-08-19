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
/// Verifies meta and lifecycle integrity after persistence and retrieval
/// </summary>
[Collection("postgres-db")]
public sealed class FormRunRepositoryLifecycleTests
{
    private readonly PostgresFixture _fx;

    /// <summary>
    /// Initializes a new instance of the test with the shared Postgres fixture
    /// </summary>
    /// <param name="fx">The Postgres fixture</param>
    public FormRunRepositoryLifecycleTests(PostgresFixture fx)
    {
        _fx = fx;
    }

    /// <summary>
    /// Create then get preserves meta and lifecycle
    /// </summary>
    [Fact(DisplayName = "Create run then get preserves meta and lifecycle integrity")]
    public async Task Create_run_then_get_preserves_meta_and_lifecycle_integrity()
    {
        await _fx.ResetAsync();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        await using var scope = _fx.Services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FormRunRepository>>();
        var repo = new FormRunRepository(factory, logger);
        DateTime start = DateTime.UtcNow.AddMinutes(-5);
        DateTime first = start.AddMinutes(1);
        DateTime last = first.AddMinutes(1);
        DateTime published = last.AddMinutes(1);
        string code = $"rf-Δ-{Guid.NewGuid():N}";
        string sup = $"комментарий ✪ {Guid.NewGuid():N}";
        var run = new FormRun
        {
            Meta = new RunMeta
            {
                Form = new RunFormRef
                {
                    FormId = System.Security.Cryptography.RandomNumberGenerator.GetInt32(1, int.MaxValue),
                    FormCode = Guid.NewGuid().ToString("N"),
                },
                RunFor = code,
                SupervisorComment = sup,
            },
            State = new RunState
            {
                Lifecycle = new RunLifecycle
                {
                    Launched = new Stamp { UserId = "u-φ", At = start },
                    FirstSaved = new Stamp { UserId = "u-φ", At = first },
                    LastSaved = new Stamp { UserId = "u-φ", At = last },
                    Published = new Stamp { UserId = "u-φ", At = published },
                },
                Context = new RunContext { Items = new Dictionary<string, string>(StringComparer.Ordinal) },
                Result = new RunResult { CurrentTotal = null, Criteria = [] },
                Agreement = null,
            },
        };
        long id = await repo.CreateAsync(run, cts.Token);
        var copy = await repo.GetAsync(id, isFullInclude: true, cts.Token);
        Assert.True(
            string.Equals(copy.Meta.SupervisorComment, sup, StringComparison.Ordinal) &&
            string.Equals(copy.Meta.RunFor, code, StringComparison.Ordinal) &&
            copy.State.Lifecycle.Published is not null && copy.State.Lifecycle.Published.At != default &&
            copy.State.Lifecycle.FirstSaved is not null && copy.State.Lifecycle.FirstSaved.At != default &&
            copy.State.Lifecycle.LastSaved is not null && copy.State.Lifecycle.LastSaved.At != default,
            $"Form run meta or lifecycle not preserved which is a failure expected non empty lifecycle timestamps actual Published={copy.State.Lifecycle.Published?.At:o} FirstSaved={copy.State.Lifecycle.FirstSaved?.At:o} LastSaved={copy.State.Lifecycle.LastSaved?.At:o} RunFor={copy.Meta.RunFor} SupervisorComment={copy.Meta.SupervisorComment}");
    }
}
