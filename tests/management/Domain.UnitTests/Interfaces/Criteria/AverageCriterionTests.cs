using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for IAverageCriterion behavioral contract.
/// </summary>
public sealed class AverageCriterionTests
{
    [Fact]
    public void Validates_as_single_criterion()
    {
        var criterion = new TestAverageCriterion(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => criterion.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_for_average_scoring()
    {
        var criterion = new TestAverageCriterion(RatingContributionTestData.MultipleContributions(), true);

        var contribution = criterion.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution_in_average_calculation()
    {
        var criterion = new TestAverageCriterion(RatingContributionTestData.EmptyContribution(), true);

        var contribution = criterion.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Inherits_validation_behavior_from_criterion()
    {
        var criterion = new TestAverageCriterion(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => criterion.Validate());
    }
}

/// <summary>
/// Test double for average criterion interface.
/// </summary>
file sealed record TestAverageCriterion(IRatingContribution TestContribution, bool IsValid) : IAverageCriterion
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
