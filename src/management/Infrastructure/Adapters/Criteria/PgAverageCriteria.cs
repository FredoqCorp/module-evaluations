using CascVel.Modules.Evaluations.Management.Domain.Entities.Criteria.Average;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;
using Dapper;

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
    public async Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, FormId formId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(ratingOptions);

        var connection = await _unitOfWork.ActiveConnection(ct);

        // Serialize rating options to JSON using Print + Output pattern
        using var jsonMedia = new Media.JsonMediaWriter();
        ratingOptions.Print(jsonMedia);
        var ratingOptionsJson = jsonMedia.Output();

        await connection.ExecuteAsync(
            new CommandDefinition(
                commandText: """
                    INSERT INTO form_criteria (id, form_id, group_id, title, text, criterion_type, weight_basis_points, rating_options, order_index, created_at)
                    VALUES (@Id, @FormId, @GroupId, @Title, @Text, @CriterionType, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)
                    """,
                parameters: new
                {
                    Id = id.Value,
                    FormId = formId.Value,
                    GroupId = (Guid?)null,
                    Title = title.Text,
                    Text = text.Text,
                    CriterionType = "average",
                    WeightBasisPoints = (int?)null,
                    RatingOptions = ratingOptionsJson,
                    OrderIndex = orderIndex.Value,
                    CreatedAt = DateTimeOffset.UtcNow
                },
                cancellationToken: ct));

        return new Criterion(id, text, title, ratingOptions);
    }

    /// <inheritdoc />
    public async Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, GroupId groupId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(ratingOptions);

        var connection = await _unitOfWork.ActiveConnection(ct);

        // Serialize rating options to JSON using Print + Output pattern
        using var jsonMedia = new Infrastructure.Media.JsonMediaWriter();
        ratingOptions.Print(jsonMedia);
        var ratingOptionsJson = jsonMedia.Output();

        await connection.ExecuteAsync(
            new CommandDefinition(
                commandText: """
                    INSERT INTO form_criteria (id, form_id, group_id, title, text, criterion_type, weight_basis_points, rating_options, order_index, created_at)
                    VALUES (@Id, @FormId, @GroupId, @Title, @Text, @CriterionType, @WeightBasisPoints, @RatingOptions::jsonb, @OrderIndex, @CreatedAt)
                    """,
                parameters: new
                {
                    Id = id.Value,
                    FormId = (Guid?)null,
                    GroupId = groupId.Value,
                    Title = title.Text,
                    Text = text.Text,
                    CriterionType = "average",
                    WeightBasisPoints = (int?)null,
                    RatingOptions = ratingOptionsJson,
                    OrderIndex = orderIndex.Value,
                    CreatedAt = DateTimeOffset.UtcNow
                },
                cancellationToken: ct));

        return new Criterion(id, text, title, ratingOptions);
    }

    /// <inheritdoc />
    public void Validate()
    {
    }
}
