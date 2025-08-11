using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Form;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal static class EvaluationFormRepositoryQueries
{
    public static IQueryable<EvaluationForm> GetFullFormQuery(this IQueryable<EvaluationForm> query)
        => query
            .AsNoTracking()
            .Include(f => f.FormCriteria);
}
