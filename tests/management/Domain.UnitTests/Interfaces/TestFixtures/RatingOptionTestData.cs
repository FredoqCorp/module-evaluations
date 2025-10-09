using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

/// <summary>
/// Test data factory for rating option scenarios.
/// </summary>
internal static class RatingOptionTestData
{
    internal static IRatingOption OptionWithScore(ushort score) =>
        new TestRatingOption(new RatingScore(score), RatingContributionTestData.SingleContribution());

    internal static IRatingOption RandomOption() =>
        new TestRatingOption(
            new RatingScore((ushort)Random.Shared.Next(1, 10)),
            RatingContributionTestData.MultipleContributions());
}

/// <summary>
/// Test double for rating option interface.
/// </summary>
internal sealed record TestRatingOption(RatingScore Score, IRatingContribution TestContribution) : IRatingOption
{
    public bool Matches(RatingScore score) =>
        Score.Value == score.Value;

    public IRatingContribution Contribution() =>
        TestContribution;
}
