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
public class EvaluationFormRepository : BaseRepository, IEvaluationFormRepository
{
    private ILogger<EvaluationFormRepository> Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EvaluationFormRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">The database context factory.</param>
    /// <param name="logger">The logger instance.</param>
    public EvaluationFormRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<EvaluationFormRepository> logger) : base(contextFactory)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<EvaluationForm> CreateAsync(EvaluationForm entity, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        await context.Database.BeginTransactionAsync(ct);

        try
        {
            context.Forms.Add(entity);
            await context.SaveChangesAsync(ct);
        }
        catch
        {
            await context.Database.RollbackTransactionAsync(ct);
            throw new InvalidDataException($"Cannot update a form with code: '{entity.Code}'");
        }

        await context.Database.CommitTransactionAsync(ct);

        return await GetAsync(entity.Id, ct: ct);
    }

    /// <inheritdoc />
    public Task DeleteAsync(long entityId, CancellationToken ct = default) => throw new NotSupportedException();

    /// <inheritdoc />
    public async Task<EvaluationForm> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return (isFullInclude
            ? await context.Forms.GetFullFormQuery().SingleOrDefaultAsync(f => f.Id == entityId, cancellationToken: ct)
            : await context.Forms.SingleOrDefaultAsync(f => f.Id == entityId, cancellationToken: ct)) ?? throw new KeyNotFoundException($"Evaluation form by id: {entityId} not found.");
    }

    /// <inheritdoc />
    public async Task<EvaluationForm?> GetByCode(string code, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return await context.Forms.SingleOrDefaultAsync(f => f.Code == code, ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<EvaluationForm>> GetListAsync(CancellationToken ct = default)
    {
        Logger.LogDebug("Geting full evaluation forms list");
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return await context.Forms.GetFullFormQuery().ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<EvaluationForm> UpdateAsync(EvaluationForm entity, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        await context.Database.BeginTransactionAsync(ct);

        try
        {
            if (entity.FormCriteria != null)
            {
                await context.BaseCriterion
                    .Where(c => EF.Property<long>(c, "EvaluationFormId") == entity.Id)
                    .ExecuteDeleteAsync(ct);
            }

            context.Forms.Update(entity);

            var entityEntry = context.Entry(entity);
            entityEntry.Property(s => s.CreatedBy).IsModified = false;
            entityEntry.Property(s => s.CreatedAt).IsModified = false;
            entityEntry.Property(s => s.Status).IsModified = false;
            entityEntry.Property(s => s.StatusChangedBy).IsModified = false;
            entityEntry.Property(s => s.StatusChangedAt).IsModified = false;

            if (entity.FormCriteria == null)
            {
                entityEntry.Collection(nameof(EvaluationForm.FormCriteria)).IsModified = false;
            }

            await context.SaveChangesAsync(ct);
        }
        catch
        {
            await context.Database.RollbackTransactionAsync(ct);
            throw new InvalidDataException($"Cannot update a form with code: '{entity.Code}'");
        }

        await context.Database.CommitTransactionAsync(ct);

        return await GetAsync(entity.Id, ct: ct);
    }

    /// <inheritdoc />
    public async Task SetStatusAsync(long id, EvaluationFormStatus status, string user, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Forms
                .Where(f => f.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(f => f.Status, status)
                    .SetProperty(f => f.StatusChangedBy, user)
                    .SetProperty(f => f.StatusChangedAt, DateTime.UtcNow)
                  , ct);
    }
}
