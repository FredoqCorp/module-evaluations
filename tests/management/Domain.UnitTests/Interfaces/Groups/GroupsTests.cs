using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IGroups behavioral contract.
/// </summary>
public sealed class GroupsTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var groups = new TestGroups(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => groups.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var groups = new TestGroups(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => groups.Validate());
    }

    [Fact]
    public void Produces_contribution_for_scoring()
    {
        var groups = new TestGroups(RatingContributionTestData.MultipleContributions(), true);

        var contribution = groups.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution()
    {
        var groups = new TestGroups(RatingContributionTestData.EmptyContribution(), true);

        var contribution = groups.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Returns_consistent_contribution_across_calls()
    {
        var expected = RatingContributionTestData.SingleContribution();
        var groups = new TestGroups(expected, true);

        var first = groups.Contribution();
        var second = groups.Contribution();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for groups interface.
/// </summary>
file sealed record TestGroups(IRatingContribution TestContribution, bool IsValid) : IGroups
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
