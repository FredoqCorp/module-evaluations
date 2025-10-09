using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for ICriteria behavioral contract.
/// </summary>
public sealed class CriteriaTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var criteria = new TestCriteria(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => criteria.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var criteria = new TestCriteria(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => criteria.Validate());
    }

    [Fact]
    public void Produces_contribution_for_scoring()
    {
        var criteria = new TestCriteria(RatingContributionTestData.MultipleContributions(), true);

        var contribution = criteria.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution()
    {
        var criteria = new TestCriteria(RatingContributionTestData.EmptyContribution(), true);

        var contribution = criteria.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Returns_consistent_contribution_across_calls()
    {
        var expected = RatingContributionTestData.SingleContribution();
        var criteria = new TestCriteria(expected, true);

        var first = criteria.Contribution();
        var second = criteria.Contribution();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for criteria interface.
/// </summary>
file sealed record TestCriteria(IRatingContribution TestContribution, bool IsValid) : ICriteria
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
