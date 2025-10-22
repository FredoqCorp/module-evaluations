using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Average;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Groups;

/// <summary>
/// PostgreSQL implementation for average groups persistence operations.
/// </summary>
internal sealed class PgAverageGroups : IAverageGroups
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes the adapter with the database unit of work.
    /// </summary>
    /// <param name="unitOfWork">Unit of work for managing database connections and transactions.</param>
    public PgAverageGroups(IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds an average group directly under a form.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added average group.</returns>
    public async Task<IAverageGroup> Add(GroupProfile profile, FormId formId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        var connection = await _unitOfWork.ActiveConnection(ct);

        try
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    commandText: """
                        INSERT INTO form_groups (id, form_id, parent_id, title, description, group_type, weight_basis_points, order_index, created_at)
                        VALUES (@Id, @FormId, @ParentId, @Title, @Description, @GroupType, @WeightBasisPoints, @OrderIndex, @CreatedAt)
                        """,
                    parameters: new
                    {
                        Id = profile.Id.Value,
                        FormId = formId.Value,
                        ParentId = (Guid?)null,
                        Title = profile.Title.Text,
                        Description = profile.Description.Text,
                        GroupType = "average",
                        WeightBasisPoints = (int?)null,
                        OrderIndex = orderIndex.Value,
                        CreatedAt = DateTimeOffset.UtcNow
                    },
                    cancellationToken: ct));
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                throw new InvalidOperationException("Database rejected average group because parent entity is missing", ex);
            }

            if (ex.SqlState == PostgresErrorCodes.CheckViolation)
            {
                throw new InvalidOperationException("Database rejected average group because type constraints failed", ex);
            }

            throw;
        }

        IAverageCriteria criteria = new AverageCriteria(Array.Empty<IAverageCriterion>());
        IAverageGroups groups = new AverageGroups(Array.Empty<IAverageGroup>());
        return new AverageCriterionGroup(profile, criteria, groups);
    }

    /// <summary>
    /// Adds an average group under another group.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="parentId">Identifier of the parent group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added average group.</returns>
    public async Task<IAverageGroup> Add(GroupProfile profile, GroupId parentId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        var connection = await _unitOfWork.ActiveConnection(ct);

        try
        {
            await connection.ExecuteAsync(
                new CommandDefinition(
                    commandText: """
                        INSERT INTO form_groups (id, form_id, parent_id, title, description, group_type, weight_basis_points, order_index, created_at)
                        VALUES (
                            @Id,
                            (SELECT form_id FROM form_groups WHERE id = @ParentId),
                            @ParentId,
                            @Title,
                            @Description,
                            @GroupType,
                            @WeightBasisPoints,
                            @OrderIndex,
                            @CreatedAt)
                        """,
                    parameters: new
                    {
                        Id = profile.Id.Value,
                        ParentId = parentId.Value,
                        Title = profile.Title.Text,
                        Description = profile.Description.Text,
                        GroupType = "average",
                        WeightBasisPoints = (int?)null,
                        OrderIndex = orderIndex.Value,
                        CreatedAt = DateTimeOffset.UtcNow
                    },
                    cancellationToken: ct));
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                throw new InvalidOperationException("Database rejected average group because parent entity is missing", ex);
            }

            if (ex.SqlState == PostgresErrorCodes.CheckViolation)
            {
                throw new InvalidOperationException("Database rejected average group because type constraints failed", ex);
            }

            if (ex.SqlState == PostgresErrorCodes.NotNullViolation)
            {
                throw new InvalidOperationException("Database rejected average group because parent entity is missing", ex);
            }

            throw;
        }

        IAverageCriteria criteria = new AverageCriteria([]);
        IAverageGroups groups = new AverageGroups([]);
        return new AverageCriterionGroup(profile, criteria, groups);
    }

    /// <inheritdoc />
    public void Validate()
    {
    }
}
