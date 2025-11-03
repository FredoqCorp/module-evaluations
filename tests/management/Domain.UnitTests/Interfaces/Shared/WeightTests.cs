using CascVel.Modules.Evaluations.Management.Domain.Models.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

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
    public void Preserves_zero_score_after_weighting()
    {
        var weight = SharedTypesTestData.RandomWeight();
        var zeroScore = new CriterionScore(0m);

        var weighted = weight.Weighted(zeroScore);

        Assert.Equal(0m, weighted.Value);
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
