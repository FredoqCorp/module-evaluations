using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;
using CascVel.Module.Evaluations.Management.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="AutomaticParameter"/> entities.
/// </summary>
internal sealed class AutomaticParameterRepository : BaseRepository, IAutomaticParameterRepository
{
    private ILogger<AutomaticParameterRepository> Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutomaticParameterRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">The database context factory.</param>
    /// <param name="logger">The logger instance.</param>
    public AutomaticParameterRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<AutomaticParameterRepository> logger) : base(contextFactory)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public async Task<long> CreateAsync(AutomaticParameter entity, CancellationToken ct = default)
    {
        Logger.LogDebug("Creating automatic parameter: Caption={Caption} ConditionType={ConditionType} ServiceCode={ServiceCode}", entity.Caption, entity.ConditionType, entity.ServiceCode.Value);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        await context.AutomaticParameters.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);

        Logger.LogInformation("Automatic parameter created: Id={Id} Caption={Caption} ConditionType={ConditionType} ServiceCode={ServiceCode}", entity.Id, entity.Caption, entity.ConditionType, entity.ServiceCode.Value);
        return entity.Id;
    }

    /// <inheritdoc />
    public Task DeleteAsync(long entityId, CancellationToken ct = default) => throw new NotSupportedException();

    /// <inheritdoc />
    public async Task<AutomaticParameter> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving automatic parameter: Id={Id} IncludeFull={IncludeFull}", entityId, isFullInclude);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        var query = context.AutomaticParameters.AsNoTracking();
        if (isFullInclude)
        {
            query = query.GetAutoPrmQuery();
        }

        return await query.SingleOrDefaultAsync(p => p.Id == entityId, ct)
               ?? throw new KeyNotFoundException($"Automatic parameter by id: {entityId} not found.");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AutomaticParameter>> GetListAsync(CancellationToken ct = default)
    {
        Logger.LogDebug("Retrieving automatic parameters list");

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        var parameters = await context.AutomaticParameters.AsNoTracking().ToListAsync(ct);

        Logger.LogDebug("Retrieved {Count} automatic parameters", parameters.Count);

        return parameters;
    }

    /// <inheritdoc />
    public async Task UpdateAsync(AutomaticParameter entity, CancellationToken ct = default)
    {
        Logger.LogDebug("Updating automatic parameter: Id={Id} Caption={Caption} ConditionType={ConditionType}", entity.Id, entity.Caption, entity.ConditionType);

        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        context.AutomaticParameters.Attach(entity);
        context.Entry(entity).State = EntityState.Modified; // Key (Id) is not updated.

        await context.SaveChangesAsync(ct);
        Logger.LogInformation("Automatic parameter updated: Id={Id}", entity.Id);
    }
}

