using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IGroup behavioral contract.
/// </summary>
public sealed class GroupTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var group = new TestGroup(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => group.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var group = new TestGroup(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => group.Validate());
    }

    [Fact]
    public void Produces_contribution_for_scoring()
    {
        var group = new TestGroup(RatingContributionTestData.MultipleContributions(), true);

        var contribution = group.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution()
    {
        var group = new TestGroup(RatingContributionTestData.EmptyContribution(), true);

        var contribution = group.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Returns_consistent_contribution_across_calls()
    {
        var expected = RatingContributionTestData.SingleContribution();
        var group = new TestGroup(expected, true);

        var first = group.Contribution();
        var second = group.Contribution();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for group interface.
/// </summary>
file sealed record TestGroup(IRatingContribution TestContribution, bool IsValid) : IGroup
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
