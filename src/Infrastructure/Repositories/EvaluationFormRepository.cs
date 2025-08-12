using System;
using CascVel.Module.Evaluations.Management.Domain.Entities.Form;
using CascVel.Module.Evaluations.Management.Domain.Enums.Forms;
using CascVel.Module.Evaluations.Management.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="EvaluationForm"/> entities.
/// </summary>
internal sealed class EvaluationFormRepository : BaseRepository, IEvaluationFormRepository
{
    private ILogger<EvaluationFormRepository> Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EvaluationFormRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">The database context factory.</param>
    /// <param name="logger">The logger instance.</param>
    public EvaluationFormRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<EvaluationFormRepository> logger) : base(contextFactory)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task<long> CreateAsync(EvaluationForm entity, CancellationToken ct = default)
    {
        Logger.LogDebug("Creating evaluation form: Code={Code} Title={Title} Status={Status}", entity.Code, entity.Title, entity.Status);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        try
        {
            await context.Forms.AddAsync(entity, ct);
            await context.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to create evaluation form with code {Code}", entity.Code);
            throw new InvalidDataException($"Cannot create a form with code: '{entity.Code}'", ex);
        }

        Logger.LogInformation("Evaluation form created: Id={Id} Code={Code} Status={Status}", entity.Id, entity.Code, entity.Status);
        return entity.Id;
    }

    /// <inheritdoc />
    public Task DeleteAsync(long entityId, CancellationToken ct = default) => throw new NotSupportedException();

    /// <inheritdoc />
    public async Task<EvaluationForm> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving evaluation form: Id={Id} IncludeFull={IncludeFull}", entityId, isFullInclude);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return (isFullInclude
            ? await context.Forms.GetFullFormQuery().SingleOrDefaultAsync(f => f.Id == entityId, cancellationToken: ct)
            : await context.Forms.SingleOrDefaultAsync(f => f.Id == entityId, cancellationToken: ct)) ?? throw new KeyNotFoundException($"Evaluation form by id: {entityId} not found.");
    }

    /// <inheritdoc />
    public async Task<EvaluationForm?> GetByCode(string code, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving evaluation form by code {Code}", code);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return await context.Forms.SingleOrDefaultAsync(f => f.Code == code, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<EvaluationForm>> GetListAsync(CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving evaluation forms list (full)");
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        var forms = await context.Forms.GetFullFormQuery().ToListAsync(ct);

        Logger.LogDebug("Retrieved {Count} evaluation forms", forms.Count);

        return forms;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(EvaluationForm entity, CancellationToken ct = default)
    {
        Logger.LogDebug("Updating evaluation form: Id={Id} Code={Code} Status={Status}", entity.Id, entity.Code, entity.Status);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        try
        {
            // If criteria provided, replace them (bulk delete existing first)
            if (entity.FormCriteria != null)
            {
                await context.BaseCriterion
                    .Where(c => EF.Property<long>(c, "EvaluationFormId") == entity.Id)
                    .ExecuteDeleteAsync(ct);
            }

            // Attach detached entity and mark as Modified
            context.Forms.Attach(entity);
            var entry = context.Entry(entity);
            entry.State = EntityState.Modified;

            // Preserve immutable / system fields
            entry.Property(s => s.CreatedBy).IsModified = false;
            entry.Property(s => s.CreatedAt).IsModified = false;
            entry.Property(s => s.Status).IsModified = false; // Status managed separately via SetStatusAsync
            entry.Property(s => s.StatusChangedBy).IsModified = false;
            entry.Property(s => s.StatusChangedAt).IsModified = false;

            if (entity.FormCriteria == null)
            {
                entry.Collection(nameof(EvaluationForm.FormCriteria)).IsModified = false;
            }

            await context.SaveChangesAsync(ct);
            Logger.LogInformation("Evaluation form updated: Id={Id}", entity.Id);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to update evaluation form {FormId}", entity.Id);
            throw new DbUpdateException($"Cannot update a form with code: '{entity.Code}'", ex);
        }
    }

    /// <inheritdoc />
    public async Task SetStatusAsync(long id, EvaluationFormStatus status, string user, CancellationToken ct = default)
    {
        Logger.LogInformation("Setting status for evaluation form {FormId} to {Status} by {User}", id, status, user);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Forms
                .Where(f => f.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(f => f.Status, status)
                    .SetProperty(f => f.StatusChangedBy, user)
                    .SetProperty(f => f.StatusChangedAt, DateTime.UtcNow)
                  , ct);

        Logger.LogInformation("Status for evaluation form {FormId} set to {Status}", id, status);
    }
}
