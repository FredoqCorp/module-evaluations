using CascVel.Module.Evaluations.Management.Domain.Entities.AutomaticParameters;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

internal static class AutomaticParameterRepositoryQueries
{
    public static IQueryable<AutomaticParameter> GetAutoPrmQuery(this IQueryable<AutomaticParameter> query)
        => query.AsNoTracking();
}
