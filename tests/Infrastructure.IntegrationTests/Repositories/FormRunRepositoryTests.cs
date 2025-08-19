using CascVel.Module.Evaluations.Management.Application.Interfaces;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Module.Evaluations.Management.Domain.Enums.Runs;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using CascVel.Module.Evaluations.Management.Infrastructure.Repositories;
using Infrastructure.IntegrationTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CascVel.Module.Evaluations.Management.Infrastructure.IntegrationTests.Repositories;

/// <summary>
/// Integration tests for FormRunRepository using real PostgreSQL via Testcontainers and EF Core migrations.
/// </summary>
[Collection("postgres-db")]
public sealed class FormRunRepositoryTests
{
    private readonly PostgresFixture _fx;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormRunRepositoryTests"/> class with the provided PostgresFixture.
    /// </summary>
    /// <param name="fx">The PostgresFixture used to set up the test database context.</param>
    public FormRunRepositoryTests(PostgresFixture fx)
    {
        _fx = fx;
    }

    /// <summary>
    /// Verifies that creating and retrieving a form run returns the same aggregate instance.
    /// Usage: This test creates a run, saves it, and asserts that the loaded run has the same RunFor value.
    /// </summary>
    [Fact(DisplayName = "Create and get form run returns the same aggregate")]
    public async Task Create_and_get_form_run_returns_the_same_aggregate()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FormRunRepository>>();
        IFormRunRepository repo = new FormRunRepository(factory, logger);
        var now = DateTime.UtcNow;
        var run = new FormRun
        {
            Meta = new RunMeta
            {
                Form = new RunFormRef { FormId = Random.Shared.Next(1, int.MaxValue), FormCode = Guid.NewGuid().ToString("N") },
                RunFor = $"r-Ω-{Guid.NewGuid():N}",
                SupervisorComment = "Комментарий ♣",
            },
            State = new RunState
            {
                Lifecycle = new RunLifecycle { Launched = new Stamp { UserId = "u-σ", At = now }, FirstSaved = null, LastSaved = null, Published = null },
                Context = new RunContext { Items = new Dictionary<string, string>() },
                Result = new RunResult { CurrentTotal = null, Criteria = new List<RunCriterionScore>() },
                Agreement = null,
            },
        };
        long id = await repo.CreateAsync(run);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        Assert.True(string.Equals(loaded.Meta.RunFor, run.Meta.RunFor, StringComparison.Ordinal),"Loaded run RunFor is different which is a failure");
    }

    /// <summary>
    /// Verifies that context and criterion scores added to a run are persisted and returned when loading the full graph.
    /// </summary>
    [Fact(DisplayName = "Create run with context and scores then get returns full state")]
    public async Task Create_run_with_context_and_scores_then_get_returns_full_state()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FormRunRepository>>();
        var repo = new FormRunRepository(factory, logger);
        var now = DateTime.UtcNow;
        var key = $"ключ-{Guid.NewGuid():N}";
        var val = $"значение-{Guid.NewGuid():N}";
        long crit = Random.Shared.NextInt64(1, long.MaxValue);
        ushort score = (ushort)Random.Shared.Next(1, 1000);
        var run = new FormRun
        {
            Meta = new RunMeta
            {
                Form = new RunFormRef { FormId = Random.Shared.Next(1, int.MaxValue), FormCode = Guid.NewGuid().ToString("N") },
                RunFor = $"r-Ψ-{Guid.NewGuid():N}",
                SupervisorComment = null,
            },
            State = new RunState
            {
                Lifecycle = new RunLifecycle { Launched = new Stamp { UserId = "u-λ", At = now }, FirstSaved = null, LastSaved = null, Published = null },
                Context = new RunContext { Items = new Dictionary<string, string> { [key] = val } },
                Result = new RunResult { CurrentTotal = 42.5m, Criteria = new List<RunCriterionScore> { new RunCriterionScore { CriterionId = crit, Skipped = false, Assessment = new CriterionAssessment { SelectedScore = score, Comment = "комментарий ☢", Auto = null } } } },
                Agreement = new RunAgreementTrail { ViewedAt = now, Status = RunAgreementStatus.Agreed, DecidedAt = now },
            },
        };
        long id = await repo.CreateAsync(run);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        bool hasContext = loaded.State.Context.Items.TryGetValue(key, out var v) && string.Equals(v, val, StringComparison.Ordinal);
        bool hasScore = loaded.State.Result.Criteria.Any(c => c.CriterionId == crit && c.Assessment?.SelectedScore == score);
        bool hasAgree = loaded.State.Agreement?.Status == RunAgreementStatus.Agreed;
        Assert.True(hasContext && hasScore && hasAgree,"Form run state does not contain expected context score or agreement which is a failure");
    }

    /// <summary>
    /// Verifies full integrity of meta and lifecycle after persistence and retrieval.
    /// </summary>
    [Fact(DisplayName = "Create run then get preserves meta and lifecycle integrity")]
    public async Task Create_run_then_get_preserves_meta_and_lifecycle_integrity()
    {
        await _fx.ResetAsync();
        await using var scope = _fx.Services.CreateAsyncScope();
        IDbContextFactory<DatabaseContext> factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DatabaseContext>>();
        ILogger<FormRunRepository> logger = scope.ServiceProvider.GetRequiredService<ILogger<FormRunRepository>>();
        var repo = new FormRunRepository(factory, logger);
        DateTime start = DateTime.UtcNow.AddMinutes(-5);
        DateTime first = start.AddMinutes(1);
        DateTime last = first.AddMinutes(1);
        DateTime publish = last.AddMinutes(1);
        string runFor = $"rf-Δ-{Guid.NewGuid():N}";
        const string sup = "комментарий ✪";
        var run = new FormRun
        {
            Meta = new RunMeta
            {
                Form = new RunFormRef { FormId = Random.Shared.Next(1, int.MaxValue), FormCode = Guid.NewGuid().ToString("N") },
                RunFor = runFor,
                SupervisorComment = sup,
            },
            State = new RunState
            {
                Lifecycle = new RunLifecycle { Launched = new Stamp { UserId = "u-φ", At = start }, FirstSaved = new Stamp { UserId = "u-φ", At = first }, LastSaved = new Stamp { UserId = "u-φ", At = last }, Published = new Stamp { UserId = "u-φ", At = publish } },
                Context = new RunContext { Items = new Dictionary<string, string>() },
                Result = new RunResult { CurrentTotal = null, Criteria = new List<RunCriterionScore>() },
                Agreement = null,
            },
        };
        long id = await repo.CreateAsync(run);
        var loaded = await repo.GetAsync(id, isFullInclude: true);
        bool sameMeta = string.Equals(loaded.Meta.SupervisorComment, sup, StringComparison.Ordinal) && string.Equals(loaded.Meta.RunFor, runFor, StringComparison.Ordinal);
        bool sameLifecycle = loaded.State.Lifecycle.Published?.At == publish && loaded.State.Lifecycle.FirstSaved?.At == first && loaded.State.Lifecycle.LastSaved?.At == last;
        Assert.True(sameMeta && sameLifecycle,"Form run meta or lifecycle not preserved which is a failure");
    }
}
