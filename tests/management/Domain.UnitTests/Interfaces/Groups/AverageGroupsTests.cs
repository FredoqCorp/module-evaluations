using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IAverageGroups behavioral contract.
/// </summary>
public sealed class AverageGroupsTests
{
    [Fact]
    public void Validates_as_groups_collection()
    {
        var groups = new TestAverageGroups(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => groups.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_for_average_scoring()
    {
        var groups = new TestAverageGroups(RatingContributionTestData.MultipleContributions(), true);

        var contribution = groups.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution_in_average_calculation()
    {
        var groups = new TestAverageGroups(RatingContributionTestData.EmptyContribution(), true);

        var contribution = groups.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Inherits_validation_behavior_from_groups()
    {
        var groups = new TestAverageGroups(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => groups.Validate());
    }
}

/// <summary>
/// Test double for average groups interface.
/// </summary>
file sealed record TestAverageGroups(IRatingContribution TestContribution, bool IsValid) : IAverageGroups
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
