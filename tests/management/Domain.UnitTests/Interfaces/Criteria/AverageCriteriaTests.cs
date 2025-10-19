using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for IAverageCriteria behavioral contract.
/// </summary>
public sealed class AverageCriteriaTests
{
    [Fact]
    public void Validates_as_criteria_collection()
    {
        var criteria = new TestAverageCriteria(true);

        var exception = Record.Exception(() => criteria.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Inherits_validation_behavior_from_criteria()
    {
        var criteria = new TestAverageCriteria(false);

        Assert.Throws<InvalidOperationException>(() => criteria.Validate());
    }
}

/// <summary>
/// Test double for average criteria interface.
/// </summary>
file sealed record TestAverageCriteria(bool IsValid) : IAverageCriteria
{
    public static Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, FormId formId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public static Task<IAverageCriterion> Add(CriterionId id, CriterionText text, CriterionTitle title, IRatingOptions ratingOptions, GroupId groupId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
