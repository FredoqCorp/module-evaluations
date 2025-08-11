using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;
using CascVel.Module.Evaluations.Management.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CascVel.Module.Evaluations.Management.Infrastructure.Context;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Repository for managing <see cref="AutomaticParameter"/> entities.
/// </summary>
public class AutomaticParameterRepository : BaseRepository, IAutomaticParameterRepository
{
    private ILogger<AutomaticParameterRepository> Logger { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutomaticParameterRepository"/> class.
    /// </summary>
    /// <param name="contextFactory">The database context factory.</param>
    /// <param name="logger">The logger instance.</param>
    public AutomaticParameterRepository(IDbContextFactory<DatabaseContext> contextFactory, ILogger<AutomaticParameterRepository> logger) : base(contextFactory)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<AutomaticParameter> CreateAsync(AutomaticParameter entity, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);
        await context.Database.BeginTransactionAsync(ct);

        try
        {
            context.AutomaticParameters.Add(entity);
            await context.SaveChangesAsync(ct);
        }
        catch
        {
            await context.Database.RollbackTransactionAsync(ct);
            throw;
        }

        await context.Database.CommitTransactionAsync(ct);

        return await GetAsync(entity.Id, ct: ct);
    }

    /// <inheritdoc />
    public Task DeleteAsync(long entityId, CancellationToken ct = default) => throw new NotSupportedException();

    /// <inheritdoc />
    public async Task<AutomaticParameter> GetAsync(long entityId, bool isFullInclude = true, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return (isFullInclude
            ? await context.AutomaticParameters.GetAutoPrmQuery().SingleOrDefaultAsync(p => p.Id == entityId, ct)
            : await context.AutomaticParameters.SingleOrDefaultAsync(p => p.Id == entityId, ct)) ?? throw new KeyNotFoundException($"Evaluation form by id: {entityId} not found.");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AutomaticParameter>> GetListAsync(CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        return await context.AutomaticParameters.ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<AutomaticParameter> UpdateAsync(AutomaticParameter entity, CancellationToken ct = default)
    {
        await using var context = await ContextFactory.CreateDbContextAsync(ct);

        await context.Database.BeginTransactionAsync(ct);

        try
        {
            context.AutomaticParameters.Update(entity);

            await context.SaveChangesAsync(ct);
        }
        catch
        {
            await context.Database.RollbackTransactionAsync(ct);
            throw;
        }

        await context.Database.CommitTransactionAsync(ct);

        return await GetAsync(entity.Id, ct: ct);
    }
}

