using System.Text.Json;
using CascVel.Module.Evaluations.Management.Domain.Entities.Filters;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Module.Evaluations.Management.Domain.Enums.Runs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using CascVel.Module.Evaluations.Management.Application.Interfaces;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Run entities and related operations.
/// </summary>
internal sealed class RunRepository : BaseRepository, IRunRepository
{
    private ILogger<RunRepository> Logger { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="RunRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">The database context factory.</param>
    /// <param name="logger">The logger instance.</param>
    public RunRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<RunRepository> logger) : base(contextFactory)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task<long> CreateAsync(Run entity, CancellationToken ct = default)
    {
        Logger.LogDebug("Creating run: RunFor={RunFor} FormId={FormId} Score={Score}", entity.RunFor, entity.EvaluationFormId, entity.ScoreResult);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        await context.Runs.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);

        Logger.LogInformation("Run created: Id={Id} RunFor={RunFor} FormId={FormId}", entity.Id, entity.RunFor, entity.EvaluationFormId);
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long entityId, CancellationToken ct = default)
    {
        Logger.LogInformation("Deleting run: Id={Id}", entityId);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        var run = await context.Runs.SingleOrDefaultAsync(r => r.Id == entityId, ct)
                  ?? throw new KeyNotFoundException($"Run by id: {entityId} not found.");
        context.Runs.Remove(run);
        await context.SaveChangesAsync(ct);

        Logger.LogInformation("Run {RunId} deleted", entityId);
    }

    /// <inheritdoc />
    public async Task<Run> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving run: Id={Id} IncludeFull={IncludeFull}", entityId, isFullInclude);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        var query = context.Runs.AsNoTracking();
        if (isFullInclude)
        {
            query = query.GetRunFormQuery();
        }
        return await query.SingleOrDefaultAsync(p => p.Id == entityId, ct) ?? throw new KeyNotFoundException($"Run by id: {entityId} not found.");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Run>> GetListAsync(RunFilter filter, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving run list with filter: RunFor={RunFor} PublishedOnly={Published} NotViewedOnly={NotViewed}", filter.RunFor, filter.OnlyPublished, filter.OnlyNotViewed);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        var query = context.Runs.GetRunFormQuery().AsNoTracking();
        if (filter.RunFor is not null)
        {
            query = query.Where(r => r.RunFor == filter.RunFor);
        }

        if (filter.RunChangeDateFrom is not null)
        {
            query = query.Where(r => r.LastSavedAt > r.PublishedAt ? r.LastSavedAt >= filter.RunChangeDateFrom : r.PublishedAt >= filter.RunChangeDateFrom);
        }

        if (filter.RunChangeDateUntil is not null)
        {
            query = query.Where(r => r.LastSavedAt > r.PublishedAt ? r.LastSavedAt <= filter.RunChangeDateUntil : r.PublishedAt <= filter.RunChangeDateUntil);
        }

        if (filter.OnlyNotViewed is not null && filter.OnlyNotViewed == true)
        {
            query = query.Where(r => r.ViewedAt == null);
        }

        if (filter.OnlyPublished is not null && filter.OnlyPublished == true)
        {
            query = query.Where(r => r.PublishedAt != null);
        }

        if (filter.Context?.Count > 0)
        {
            string jsonstring = JsonSerializer.Serialize(filter.Context);
            query = query.Where(r => r.Context != null && EF.Functions.JsonContains(r.Context!, jsonstring));
        }

        var runs = await query.ToListAsync(cancellationToken: ct);

        Logger.LogDebug("Retrieved {Count} runs", runs.Count);

        return runs;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Run entity, CancellationToken ct = default)
    {
        Logger.LogDebug("Updating run: Id={Id} RunFor={RunFor} FormId={FormId}", entity.Id, entity.RunFor, entity.EvaluationFormId);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        await using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            // Replace criterion results (bulk delete existing)
            await context.Results
                .Where(c => EF.Property<long>(c, "RunId") == entity.Id)
                .ExecuteDeleteAsync(ct);

            // Attach detached entity and mark Modified
            context.Runs.Attach(entity);
            var entry = context.Entry(entity);
            entry.State = EntityState.Modified;

            // Preserve immutable / system-managed fields
            entry.Property(s => s.CreatedBy).IsModified = false;
            entry.Property(s => s.CreatedAt).IsModified = false;
            entry.Property(s => s.Context).IsModified = false;
            entry.Property(s => s.EvaluationFormId).IsModified = false;
            entry.Property(s => s.PublishedAt).IsModified = false;
            entry.Property(s => s.PublishedBy).IsModified = false;
            entry.Property(s => s.RunFor).IsModified = false;
            entry.Property(s => s.FirstSavedAt).IsModified = false;
            entry.Property(s => s.FirstSavedBy).IsModified = false;

            await context.SaveChangesAsync(ct);

            // FirstSavedAt/FirstSavedBy initialization if first save scenario
            bool needsFirstSave = await context.Runs
                .Where(r => r.Id == entity.Id && r.FirstSavedAt == null)
                .AnyAsync(ct);
            if (needsFirstSave)
            {
                await context.Runs
                    .Where(r => r.Id == entity.Id && r.FirstSavedAt == null)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(r => r.FirstSavedBy, r => r.LastSavedBy)
                        .SetProperty(r => r.FirstSavedAt, r => r.LastSavedAt)
                      , ct);
            }

            await transaction.CommitAsync(ct);
            Logger.LogInformation("Run updated: Id={Id}", entity.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            Logger.LogError(ex, "Failed to update run {RunId}", entity.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task Publish(long id, string userLogin, CancellationToken ct = default)
    {
        Logger.LogInformation("Publishing run: Id={Id} User={User}", id, userLogin);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Runs
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(r => r.PublishedBy, userLogin)
                    .SetProperty(r => r.PublishedAt, DateTime.UtcNow)
                   , ct);

        Logger.LogInformation("Run published: Id={Id}", id);
    }

    /// <inheritdoc />
    public async Task SetRunViewedState(long id, CancellationToken ct = default)
    {
        Logger.LogInformation("Marking run as viewed: Id={Id}", id);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Runs
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(setters => setters
                        .SetProperty(r => r.ViewedAt, DateTime.UtcNow)
                  , ct);
    }
    /// <summary>
    /// Set approve state for a run.
    /// </summary>
    public async Task SetApproveState(long id, RunAgreementStatus status, CancellationToken ct = default)
    {
        Logger.LogInformation("Setting approve state for run: Id={Id} Status={Status}", id, status);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Runs
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(setters => setters
                        .SetProperty(r => r.AgreementStatus, status)
                        .SetProperty(r => r.AgreementAt, DateTime.UtcNow)
                  , ct);
    }
}
