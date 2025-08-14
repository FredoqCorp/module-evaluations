using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal static class EvaluationFormRepositoryQueries
{
    public static IQueryable<EvaluationForm> WithDesignGraph(this IQueryable<EvaluationForm> query)
        => query
            .AsNoTracking()
            .AsSplitQuery()
            .Include(f => f.Design.Groups)
            .ThenInclude(g => g.Criteria)
            .ThenInclude(c => c.Criterion);
}
