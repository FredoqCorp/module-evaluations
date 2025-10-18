using System;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Criteria;

/// <summary>
/// PostgreSQL implementation for criteria.
/// </summary>
internal sealed class PgAverageCriteria : IAverageCriteria
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes the adapter with the database unit of work.
    /// </summary>
    /// <param name="unitOfWork">Unit of work for managing database connections and transactions.</param>
    public PgAverageCriteria(IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    public static async Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public void Validate()
    {
        throw new NotImplementedException();
    }
}
