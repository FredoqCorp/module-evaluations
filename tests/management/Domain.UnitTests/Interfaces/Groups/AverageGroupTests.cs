using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IAverageGroup behavioral contract.
/// </summary>
public sealed class AverageGroupTests
{
    [Fact]
    public void Validates_as_group()
    {
        var group = new TestAverageGroup(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => group.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_for_average_scoring()
    {
        var group = new TestAverageGroup(RatingContributionTestData.MultipleContributions(), true);

        var contribution = group.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution_in_average_calculation()
    {
        var group = new TestAverageGroup(RatingContributionTestData.EmptyContribution(), true);

        var contribution = group.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Inherits_validation_behavior_from_group()
    {
        var group = new TestAverageGroup(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => group.Validate());
    }
}

/// <summary>
/// Test double for average group interface.
/// </summary>
file sealed record TestAverageGroup(IRatingContribution TestContribution, bool IsValid) : IAverageGroup
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
