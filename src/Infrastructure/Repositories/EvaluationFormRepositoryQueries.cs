using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;

namespace CascVel.Module.Evaluations.Management.Infrastructure.Repositories;

/// <summary>
/// Query helpers for EvaluationForm repository
/// </summary>
internal static class EvaluationFormRepositoryQueries
{
    /// <summary>
    /// Includes the full aggregate graph for read-only scenarios
    /// </summary>
    public static IQueryable<EvaluationForm> WithGraph(this IQueryable<EvaluationForm> query)
        => query
            .AsNoTracking()
            .Include(f => f.Groups)
            .Include(f => f.Criteria)
            .AsSplitQuery();
}
