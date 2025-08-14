using CascVel.Module.Evaluations.Management.Application.Interfaces;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal sealed class EvaluationFormRepository : IEvaluationFormRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly ILogger<EvaluationFormRepository> _logger;

    public EvaluationFormRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<EvaluationFormRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task<long> CreateAsync(EvaluationForm entity, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        await db.EvaluationForms.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        _logger.LogInformation("Form created: Id={Id} Code={Code}", entity.Id, entity.Meta.Code.Value);
        return entity.Id;
    }

    public async Task DeleteAsync(long entityId, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var existing = await db.EvaluationForms.SingleOrDefaultAsync(f => f.Id == entityId, ct)
            ?? throw new KeyNotFoundException($"EvaluationForm by id {entityId} not found");
        db.EvaluationForms.Remove(existing);
        await db.SaveChangesAsync(ct);
        _logger.LogInformation("Form deleted: Id={Id}", entityId);
    }

    public async Task<EvaluationForm> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var q = isFullInclude
            ? db.EvaluationForms.WithDesignGraph()
            : db.EvaluationForms.AsNoTracking();
        return await q.SingleOrDefaultAsync(f => f.Id == entityId, ct)
               ?? throw new KeyNotFoundException($"EvaluationForm by id {entityId} not found");
    }
}
