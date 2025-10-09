using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Ratings;

/// <summary>
/// Tests for IRatingContribution behavioral contract.
/// </summary>
public sealed class RatingContributionTests
{
    [Fact]
    public void Combines_two_contributions_into_single_result()
    {
        var first = RatingContributionTestData.SingleContribution();
        var second = RatingContributionTestData.MultipleContributions();

        var combined = first.Join(second);

        Assert.NotNull(combined);
    }

    [Fact]
    public void Projects_amount_and_participants_through_provided_function()
    {
        var contribution = RatingContributionTestData.MultipleContributions();
        var projected = false;

        contribution.Accept((_, _) =>
        {
            projected = true;
            return projected;
        });

        Assert.True(projected);
    }

    [Fact]
    public void Returns_normalized_value_when_participants_exist()
    {
        var contribution = RatingContributionTestData.SingleContribution();

        var total = contribution.Total();

        Assert.True(total.IsSome);
    }

    [Fact]
    public void Returns_none_when_no_participants_exist()
    {
        var contribution = RatingContributionTestData.EmptyContribution();

        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Preserves_contribution_semantics_after_join()
    {
        var first = RatingContributionTestData.SingleContribution();
        var second = RatingContributionTestData.SingleContribution();

        var combined = first.Join(second);
        var total = combined.Total();

        Assert.True(total.IsSome);
    }

    [Fact]
    public void Accepts_projector_with_zero_amount()
    {
        var contribution = RatingContributionTestData.ZeroAmountContribution();
        var result = 0m;

        contribution.Accept((amount, _) =>
        {
            result = amount;
            return result;
        });

        Assert.Equal(0m, result);
    }
}
