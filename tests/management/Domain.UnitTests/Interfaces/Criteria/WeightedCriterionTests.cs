using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for IWeightedCriterion behavioral contract.
/// </summary>
public sealed class WeightedCriterionTests
{
    [Fact]
    public void Returns_associated_weight()
    {
        var criterion = new TestWeightedCriterion(
            RatingContributionTestData.SingleContribution(),
            SharedTypesTestData.RandomWeight(),
            true);

        var weight = criterion.Weight();

        Assert.NotNull(weight);
    }

    [Fact]
    public void Validates_as_weighted_criterion()
    {
        var criterion = new TestWeightedCriterion(
            RatingContributionTestData.SingleContribution(),
            SharedTypesTestData.RandomWeight(),
            true);

        var exception = Record.Exception(() => criterion.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_with_weighting()
    {
        var criterion = new TestWeightedCriterion(
            RatingContributionTestData.MultipleContributions(),
            SharedTypesTestData.RandomWeight(),
            true);

        var contribution = criterion.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Returns_consistent_weight_across_calls()
    {
        var expectedWeight = SharedTypesTestData.RandomWeight();
        var criterion = new TestWeightedCriterion(
            RatingContributionTestData.SingleContribution(),
            expectedWeight,
            true);

        var first = criterion.Weight();
        var second = criterion.Weight();

        Assert.Equal(first, second);
    }

    [Fact]
    public void Inherits_validation_behavior_from_criterion()
    {
        var criterion = new TestWeightedCriterion(
            RatingContributionTestData.SingleContribution(),
            SharedTypesTestData.RandomWeight(),
            false);

        Assert.Throws<InvalidOperationException>(() => criterion.Validate());
    }
}

/// <summary>
/// Test double for weighted criterion interface.
/// </summary>
file sealed record TestWeightedCriterion(
    IRatingContribution TestContribution,
    IWeight CriterionWeight,
    bool IsValid) : IWeightedCriterion
{
    public IWeight Weight() => CriterionWeight;

    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }

    public IRatingContribution Contribution() => TestContribution;
}
