using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for IAverageCriteria behavioral contract.
/// </summary>
public sealed class AverageCriteriaTests
{
    [Fact]
    public void Validates_as_criteria_collection()
    {
        var criteria = new TestAverageCriteria(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => criteria.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_for_average_scoring()
    {
        var criteria = new TestAverageCriteria(RatingContributionTestData.MultipleContributions(), true);

        var contribution = criteria.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution_in_average_calculation()
    {
        var criteria = new TestAverageCriteria(RatingContributionTestData.EmptyContribution(), true);

        var contribution = criteria.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Inherits_validation_behavior_from_criteria()
    {
        var criteria = new TestAverageCriteria(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => criteria.Validate());
    }
}

/// <summary>
/// Test double for average criteria interface.
/// </summary>
file sealed record TestAverageCriteria(IRatingContribution TestContribution, bool IsValid) : IAverageCriteria
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }

    public IRatingContribution Contribution() => TestContribution;
}
