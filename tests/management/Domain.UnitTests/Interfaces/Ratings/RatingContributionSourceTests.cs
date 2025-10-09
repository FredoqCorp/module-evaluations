using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Ratings;

/// <summary>
/// Tests for IRatingContributionSource behavioral contract.
/// </summary>
public sealed class RatingContributionSourceTests
{
    [Fact]
    public void Produces_contribution_for_downstream_scoring()
    {
        var source = new TestContributionSource(RatingContributionTestData.SingleContribution());

        var contribution = source.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Returns_consistent_contribution_on_multiple_calls()
    {
        var expected = RatingContributionTestData.MultipleContributions();
        var source = new TestContributionSource(expected);

        var first = source.Contribution();
        var second = source.Contribution();

        Assert.Equal(first, second);
    }

    [Fact]
    public void Supports_empty_contribution()
    {
        var source = new TestContributionSource(RatingContributionTestData.EmptyContribution());

        var contribution = source.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }
}

/// <summary>
/// Test double for contribution source interface.
/// </summary>
file sealed record TestContributionSource(IRatingContribution TestContribution) : IRatingContributionSource
{
    public IRatingContribution Contribution() => TestContribution;
}
