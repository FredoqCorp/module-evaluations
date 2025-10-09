using CascVel.Modules.Evaluations.Management.Domain.Common;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

/// <summary>
/// Test data factory for rating contribution test scenarios.
/// </summary>
internal static class RatingContributionTestData
{
    internal static IRatingContribution EmptyContribution() =>
        new TestRatingContribution(0m, 0);

    internal static IRatingContribution SingleContribution() =>
        new TestRatingContribution(Random.Shared.Next(1, 100), 1);

    internal static IRatingContribution MultipleContributions() =>
        new TestRatingContribution(Random.Shared.Next(100, 500), (ushort)Random.Shared.Next(2, 10));

    internal static IRatingContribution ZeroAmountContribution() =>
        new TestRatingContribution(0m, (ushort)Random.Shared.Next(1, 5));
}

/// <summary>
/// Test double for rating contribution interface.
/// </summary>
internal sealed record TestRatingContribution(decimal Amount, ushort Participants) : IRatingContribution
{
    public IRatingContribution Join(IRatingContribution contribution) =>
        contribution.Accept((amount, participants) =>
            new TestRatingContribution(Amount + amount, (ushort)(Participants + participants)));

    public T Accept<T>(Func<decimal, ushort, T> projector) =>
        projector(Amount, Participants);

    public Option<decimal> Total() =>
        Participants > 0 ? Option.Of(Amount / Participants) : Option.None<decimal>();
}
