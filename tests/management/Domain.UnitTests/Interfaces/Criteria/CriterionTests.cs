using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for ICriterion behavioral contract.
/// </summary>
public sealed class CriterionTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var criterion = new TestCriterion(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => criterion.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var criterion = new TestCriterion(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => criterion.Validate());
    }

    [Fact]
    public void Produces_contribution_for_scoring()
    {
        var criterion = new TestCriterion(RatingContributionTestData.MultipleContributions(), true);

        var contribution = criterion.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution()
    {
        var criterion = new TestCriterion(RatingContributionTestData.EmptyContribution(), true);

        var contribution = criterion.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Returns_consistent_contribution_across_calls()
    {
        var expected = RatingContributionTestData.SingleContribution();
        var criterion = new TestCriterion(expected, true);

        var first = criterion.Contribution();
        var second = criterion.Contribution();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for criterion interface.
/// </summary>
file sealed record TestCriterion(IRatingContribution TestContribution, bool IsValid) : ICriterion
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
