using CascVel.Module.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal static class FormRunRepositoryQueries
{
    public static IQueryable<FormRun> WithGraph(this IQueryable<FormRun> query)
        => query
            .AsNoTracking()
            .AsSplitQuery();
}
