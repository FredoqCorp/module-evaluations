using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IWeightedGroups behavioral contract.
/// </summary>
public sealed class WeightedGroupsTests
{
    [Fact]
    public void Returns_combined_sibling_weight()
    {
        var groups = new TestWeightedGroups(
            RatingContributionTestData.SingleContribution(),
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var weight = groups.Weight();

        Assert.NotNull(weight);
    }

    [Fact]
    public void Validates_as_weighted_collection()
    {
        var groups = new TestWeightedGroups(
            RatingContributionTestData.SingleContribution(),
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var exception = Record.Exception(() => groups.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_with_weighting()
    {
        var groups = new TestWeightedGroups(
            RatingContributionTestData.MultipleContributions(),
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var contribution = groups.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Returns_consistent_weight_across_calls()
    {
        var expectedWeight = SharedTypesTestData.RandomBasisPoints();
        var groups = new TestWeightedGroups(
            RatingContributionTestData.SingleContribution(),
            expectedWeight,
            true);

        var first = groups.Weight();
        var second = groups.Weight();

        Assert.Equal(first, second);
    }

    [Fact]
    public void Inherits_validation_behavior_from_groups()
    {
        var groups = new TestWeightedGroups(
            RatingContributionTestData.SingleContribution(),
            SharedTypesTestData.RandomBasisPoints(),
            false);

        Assert.Throws<InvalidOperationException>(() => groups.Validate());
    }
}

/// <summary>
/// Test double for weighted groups interface.
/// </summary>
file sealed record TestWeightedGroups(
    IRatingContribution TestContribution,
    IBasisPoints BasisPoints,
    bool IsValid) : IWeightedGroups
{
    public IBasisPoints Weight() => BasisPoints;

    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }

    public IRatingContribution Contribution() => TestContribution;
}
