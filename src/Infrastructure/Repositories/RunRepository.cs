using System;
using System.Text.Json;
using CascVel.Module.Evaluations.Management.Domain.Entities.Filters;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Module.Evaluations.Management.Domain.Enums.Runs;
using CascVel.Module.Evaluations.Management.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Run entities and related operations.
/// </summary>
public class RunRepository : BaseRepository, IRunRepository
{
    private ILogger<RunRepository> Logger { get; }
    /// <summary>
    /// Initializes a new instance of the <see cref="RunRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">The database context factory.</param>
    /// <param name="logger">The logger instance.</param>
    public RunRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<RunRepository> logger) : base(contextFactory)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<Run> CreateAsync(Run entity, CancellationToken ct = default)
    {
        Logger.LogInformation("Creating run {@Run}", entity);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Database.BeginTransactionAsync(ct);

        try
        {
            context.Runs.Add(entity);
            await context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync(ct);
            Logger.LogError(ex, "Failed to create run {RunId}", entity.Id);
            throw;
        }

        await context.Database.CommitTransactionAsync(ct);

        Logger.LogInformation("Run {RunId} created", entity.Id);

        return await GetAsync(entity.Id, ct: ct);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long entityId, CancellationToken ct = default)
    {
        Logger.LogInformation("Deleting run {RunId}", entityId);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Database.BeginTransactionAsync(ct);

        try
        {
            context.Runs.Remove(await GetAsync(entityId, ct: ct));
            await context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync(ct);
            Logger.LogError(ex, "Failed to delete run {RunId}", entityId);
            throw;
        }

        await context.Database.CommitTransactionAsync(ct);

        Logger.LogInformation("Run {RunId} deleted", entityId);
    }

    /// <inheritdoc />
    public async Task<Run> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving run {RunId} with full include {IsFullInclude}", entityId, isFullInclude);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return (isFullInclude
            ? await context.Runs.GetRunFormQuery().SingleOrDefaultAsync(p => p.Id == entityId, ct)
            : await context.Runs.SingleOrDefaultAsync(p => p.Id == entityId, ct)) ?? throw new KeyNotFoundException($"Run by id: {entityId} not found.");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Run>> GetListAsync(RunFilter filter, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving run list with filter {@Filter}", filter);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        var query = context.Runs.GetRunFormQuery();
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
    public async Task<Run> UpdateAsync(Run entity, CancellationToken ct = default)
    {
        Logger.LogInformation("Updating run {RunId}", entity.Id);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Database.BeginTransactionAsync(ct);

        try
        {
            await context.Results
                .Where(c => EF.Property<long>(c, "RunId") == entity.Id)
                .ExecuteDeleteAsync(ct);

            context.Runs.Update(entity);

            var entityEntry = context.Entry(entity);

            entityEntry.Property(s => s.CreatedBy).IsModified = false;
            entityEntry.Property(s => s.CreatedAt).IsModified = false;
            entityEntry.Property(s => s.Context).IsModified = false;
            entityEntry.Property(s => s.EvaluationFormId).IsModified = false;
            entityEntry.Property(s => s.PublishedAt).IsModified = false;
            entityEntry.Property(s => s.PublishedBy).IsModified = false;
            entityEntry.Property(s => s.RunFor).IsModified = false;
            entityEntry.Property(s => s.FirstSavedAt).IsModified = false;
            entityEntry.Property(s => s.FirstSavedBy).IsModified = false;


            await context.SaveChangesAsync(ct);

            var run = await context.Runs.SingleOrDefaultAsync(r => r.Id == entity.Id && r.FirstSavedAt == null, ct);

            if (run != null && run.FirstSavedAt == null)
            {
                await context.Runs
                        .Where(r => r.Id == entity.Id && r.FirstSavedAt == null)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(r => r.FirstSavedBy, r => r.LastSavedBy)
                            .SetProperty(r => r.FirstSavedAt, r => r.LastSavedAt)
                          , ct);
            }

        }
        catch (Exception ex)
        {
            await context.Database.RollbackTransactionAsync(ct);
            Logger.LogError(ex, "Failed to update run {RunId}", entity.Id);
            throw;
        }

        await context.Database.CommitTransactionAsync(ct);

        Logger.LogInformation("Run {RunId} updated", entity.Id);

        return await GetAsync(entity.Id, ct: ct);
    }

    /// <inheritdoc />
    public async Task Publish(long id, string userLogin, CancellationToken ct = default)
    {
        Logger.LogInformation("Publishing run {RunId} by {User}", id, userLogin);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Runs
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(r => r.PublishedBy, userLogin)
                    .SetProperty(r => r.PublishedAt, DateTime.UtcNow)
                   , ct);

        Logger.LogInformation("Run {RunId} published", id);
    }

    /// <inheritdoc />
    public async Task SetRunViewedState(long id, CancellationToken ct = default)
    {
        Logger.LogInformation("Marking run {RunId} as viewed", id);

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
        Logger.LogInformation("Setting approve state for run {RunId} to {Status}", id, status);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Runs
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(setters => setters
                        .SetProperty(r => r.AgreementStatus, status)
                        .SetProperty(r => r.AgreementAt, DateTime.UtcNow)
                  , ct);
    }
}
