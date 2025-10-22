using System;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Entities.Groups.Weighted;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Adapters.Groups;

/// <summary>
/// PostgreSQL implementation for weighted groups persistence operations.
/// </summary>
internal sealed class PgWeightedGroups : IWeightedGroups
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes the adapter with the database unit of work.
    /// </summary>
    /// <param name="unitOfWork">Unit of work for managing database connections and transactions.</param>
    public PgWeightedGroups(IUnitOfWork unitOfWork)
    {
        ArgumentNullException.ThrowIfNull(unitOfWork);
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds a weighted group directly under a form.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="formId">Identifier of the parent form.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted group.</returns>
    public async Task<IWeightedGroup> Add(GroupProfile profile, FormId formId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(weight);

        var connection = await _unitOfWork.ActiveConnection(ct);

        var basisPoints = decimal.ToInt32(weight.Percent().Basis().Apply(10000m));

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
                        GroupType = "weighted",
                        WeightBasisPoints = basisPoints,
                        OrderIndex = orderIndex.Value,
                        CreatedAt = DateTimeOffset.UtcNow
                    },
                    cancellationToken: ct));
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                throw new InvalidOperationException("Database rejected weighted group because parent entity is missing", ex);
            }

            if (ex.SqlState == PostgresErrorCodes.CheckViolation)
            {
                throw new InvalidOperationException("Database rejected weighted group because type constraints failed", ex);
            }

            throw;
        }

        IWeightedCriteria criteria = new WeightedCriteria(Array.Empty<IWeightedCriterion>());
        IWeightedGroups groups = new WeightedGroups(Array.Empty<IWeightedGroup>());
        return new WeightedCriterionGroup(profile, criteria, groups, weight);
    }

    /// <summary>
    /// Adds a weighted group under another group.
    /// </summary>
    /// <param name="profile">Immutable profile describing the group.</param>
    /// <param name="parentId">Identifier of the parent group.</param>
    /// <param name="weight">Weight assigned to the group.</param>
    /// <param name="orderIndex">Display order within the parent context.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The added weighted group.</returns>
    public async Task<IWeightedGroup> Add(GroupProfile profile, GroupId parentId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(weight);

        var connection = await _unitOfWork.ActiveConnection(ct);

        var basisPoints = decimal.ToInt32(weight.Percent().Basis().Apply(10000m));

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
                        GroupType = "weighted",
                        WeightBasisPoints = basisPoints,
                        OrderIndex = orderIndex.Value,
                        CreatedAt = DateTimeOffset.UtcNow
                    },
                    cancellationToken: ct));
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                throw new InvalidOperationException("Database rejected weighted group because parent entity is missing", ex);
            }

            if (ex.SqlState == PostgresErrorCodes.CheckViolation)
            {
                throw new InvalidOperationException("Database rejected weighted group because type constraints failed", ex);
            }

            if (ex.SqlState == PostgresErrorCodes.NotNullViolation)
            {
                throw new InvalidOperationException("Database rejected weighted group because parent entity is missing", ex);
            }

            throw;
        }

        IWeightedCriteria criteria = new WeightedCriteria(Array.Empty<IWeightedCriterion>());
        IWeightedGroups groups = new WeightedGroups(Array.Empty<IWeightedGroup>());
        return new WeightedCriterionGroup(profile, criteria, groups, weight);
    }

    /// <inheritdoc />
    public IBasisPoints Weight()
    {
        throw new NotImplementedException("Weight calculation is not supported in the adapter");
    }

    /// <inheritdoc />
    public void Validate()
    {
    }
}
