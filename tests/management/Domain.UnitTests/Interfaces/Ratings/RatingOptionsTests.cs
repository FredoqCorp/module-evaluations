using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Ratings;

/// <summary>
/// Tests for IRatingOptions behavioral contract.
/// </summary>
public sealed class RatingOptionsTests
{
    [Fact]
    public void Produces_contribution_from_selected_option()
    {
        var options = new TestRatingOptions(RatingContributionTestData.SingleContribution());

        var contribution = options.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution_when_no_selection()
    {
        var options = new TestRatingOptions(RatingContributionTestData.EmptyContribution());

        var contribution = options.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Returns_consistent_contribution_across_calls()
    {
        var expected = RatingContributionTestData.MultipleContributions();
        var options = new TestRatingOptions(expected);

        var first = options.Contribution();
        var second = options.Contribution();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for rating options interface.
/// </summary>
file sealed record TestRatingOptions(IRatingContribution TestContribution) : IRatingOptions
{
    public IRatingContribution Contribution() => TestContribution;
}
