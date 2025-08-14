using CascVel.Module.Evaluations.Management.Application.Interfaces;
using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal sealed class FormRunRepository : IFormRunRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly ILogger<FormRunRepository> _logger;

    public FormRunRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<FormRunRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<long> CreateAsync(FormRun entity, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        await db.FormRuns.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        _logger.LogInformation("FormRun created: Id={Id} RunFor={RunFor}", entity.Id, entity.Meta.RunFor);
        return entity.Id;
    }

    public async Task DeleteAsync(long entityId, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var existing = await db.FormRuns.SingleOrDefaultAsync(f => f.Id == entityId, ct)
            ?? throw new KeyNotFoundException($"FormRun by id {entityId} not found");
        db.FormRuns.Remove(existing);
        await db.SaveChangesAsync(ct);
        _logger.LogInformation("FormRun deleted: Id={Id}", entityId);
    }

    public async Task<FormRun> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var q = isFullInclude
            ? db.FormRuns.WithGraph()
            : db.FormRuns.AsNoTracking();
        return await q.SingleOrDefaultAsync(f => f.Id == entityId, ct)
               ?? throw new KeyNotFoundException($"FormRun by id {entityId} not found");
    }
}
