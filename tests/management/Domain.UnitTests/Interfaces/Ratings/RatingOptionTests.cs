using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Ratings;

/// <summary>
/// Tests for IRatingOption behavioral contract.
/// </summary>
public sealed class RatingOptionTests
{
    [Fact]
    public void Confirms_match_when_scores_are_identical()
    {
        var score = new RatingScore((ushort)Random.Shared.Next(1, 10));
        var option = RatingOptionTestData.OptionWithScore(score.Value);

        var matches = option.Matches(score);

        Assert.True(matches);
    }

    [Fact]
    public void Denies_match_when_scores_differ()
    {
        var option = RatingOptionTestData.OptionWithScore(5);
        var differentScore = new RatingScore(3);

        var matches = option.Matches(differentScore);

        Assert.False(matches);
    }
}
