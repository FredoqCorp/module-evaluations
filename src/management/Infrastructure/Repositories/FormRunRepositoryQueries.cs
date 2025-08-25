using CascVel.Modules.Evaluations.Management.Domain.Entities.Runs;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Repositories;

internal static class FormRunRepositoryQueries
{
    public static IQueryable<FormRun> WithGraph(this IQueryable<FormRun> query)
        => query
            .AsNoTracking()
            .AsSplitQuery();
}
