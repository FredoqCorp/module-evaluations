using CascVel.Module.Evaluations.Management.Application.Interfaces;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Concrete repository for the EvaluationForm aggregate responsible for persistence operations
/// </summary>
internal sealed class EvaluationFormRepository : IEvaluationFormRepository
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly ILogger<EvaluationFormRepository> _logger;

    /// <summary>
    /// Creates repository with a DbContext factory and logger
    /// </summary>
    public EvaluationFormRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<EvaluationFormRepository> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    /// <summary>
    /// Persists a new EvaluationForm and returns its generated identifier
    /// </summary>
    public async Task<long> CreateAsync(EvaluationForm entity, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        await db.EvaluationForms.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        _logger.LogInformation("Form created: Id={Id} Code={Code}", entity.Id, entity.Meta.Code.Value);
        return entity.Id;
    }

    /// <summary>
    /// Deletes an existing EvaluationForm by identifier
    /// </summary>
    public async Task DeleteAsync(long entityId, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var existing = await db.EvaluationForms.SingleOrDefaultAsync(f => f.Id == entityId, ct)
            ?? throw new KeyNotFoundException($"EvaluationForm by id {entityId} not found");
        db.EvaluationForms.Remove(existing);
        await db.SaveChangesAsync(ct);
        _logger.LogInformation("Form deleted: Id={Id}", entityId);
    }

    /// <summary>
    /// Retrieves an EvaluationForm by identifier optionally including related graph
    /// </summary>
    public async Task<EvaluationForm> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        await using var db = await _contextFactory.CreateDbContextAsync(ct);
        var query = db.EvaluationForms.AsNoTracking();


        if (isFullInclude)
        {
            var form = await query.Include(f => f.Criteria).ThenInclude(c => c.Criterion).SingleOrDefaultAsync(f => f.Id == entityId, ct) ?? throw new KeyNotFoundException($"EvaluationForm by id {entityId} not found");
            var groups = await db.FormGroups.Where(g => g.FormId == entityId).Include(g => g.Criteria).ThenInclude(c => c.Criterion).AsSplitQuery().AsNoTracking().ToListAsync(ct);
            var roots = groups.Where(g => g.ParentId == null).ToList();
            var map = groups.Where(g => g.ParentId != null).GroupBy(g => g.ParentId!.Value).ToDictionary(g => g.Key, g => g.ToList());
            if (roots.Count > 0)
            {
                form.AddGroups(roots);
                foreach (var r in roots)
                {
                    Link(r);
                }
            }
            return form;
            void Link(FormGroup node)
            {
                if (!map.TryGetValue(node.Id, out var kids))
                {
                    return;
                }
                node.AddChilds(kids);
                foreach (var k in kids)
                {
                    Link(k);
                }
            }
        }
        return await query.SingleOrDefaultAsync(f => f.Id == entityId, ct) ?? throw new KeyNotFoundException($"EvaluationForm by id {entityId} not found");
    }
}
