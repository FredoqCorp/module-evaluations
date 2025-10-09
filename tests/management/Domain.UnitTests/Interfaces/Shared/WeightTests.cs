using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Shared;

/// <summary>
/// Tests for IWeight behavioral contract.
/// </summary>
public sealed class WeightTests
{
    [Fact]
    public void Exposes_percent_representation()
    {
        var weight = SharedTypesTestData.RandomWeight();

        var percent = weight.Percent();

        Assert.NotNull(percent);
    }

    [Fact]
    public void Applies_weight_to_criterion_score()
    {
        var weight = SharedTypesTestData.RandomWeight();
        var score = new CriterionScore((decimal)Random.Shared.Next(1, 100));

        var weighted = weight.Weighted(score);

        Assert.True(weighted.Value >= 0);
    }

    [Fact]
    public void Applies_weight_to_contribution()
    {
        var weight = SharedTypesTestData.RandomWeight();
        var contribution = RatingContributionTestData.MultipleContributions();

        var weighted = weight.Weighted(contribution);

        Assert.NotNull(weighted);
    }

    [Fact]
    public void Preserves_zero_score_after_weighting()
    {
        var weight = SharedTypesTestData.RandomWeight();
        var zeroScore = new CriterionScore(0m);

        var weighted = weight.Weighted(zeroScore);

        Assert.Equal(0m, weighted.Value);
    }

    [Fact]
    public void Preserves_participant_count_when_weighting_contribution()
    {
        var weight = SharedTypesTestData.RandomWeight();
        var contribution = RatingContributionTestData.MultipleContributions();
        var originalParticipants = contribution.Accept((_, p) => p);

        var weighted = weight.Weighted(contribution);
        var weightedParticipants = weighted.Accept((_, p) => p);

        Assert.Equal(originalParticipants, weightedParticipants);
    }

    [Fact]
    public void Returns_consistent_percent_across_calls()
    {
        var weight = SharedTypesTestData.RandomWeight();

        var first = weight.Percent();
        var second = weight.Percent();

        Assert.Equal(first, second);
    }
}
