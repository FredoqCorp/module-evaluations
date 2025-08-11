using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal static class RunRepositoryQueries
{
    public static IQueryable<Run> GetRunFormQuery(this IQueryable<Run> query)
        => query
            .AsNoTracking()
            .Include(r => r.RunCriterionResults);
}
