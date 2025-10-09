using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Forms;

/// <summary>
/// Tests for IFormRootGroup behavioral contract.
/// </summary>
public sealed class FormRootGroupTests
{
    [Fact]
    public void Validates_as_group()
    {
        var rootGroup = new TestFormRootGroup(RatingContributionTestData.SingleContribution(), true);

        var exception = Record.Exception(() => rootGroup.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Produces_contribution_for_form_structure()
    {
        var rootGroup = new TestFormRootGroup(RatingContributionTestData.MultipleContributions(), true);

        var contribution = rootGroup.Contribution();

        Assert.NotNull(contribution);
    }

    [Fact]
    public void Supports_empty_contribution()
    {
        var rootGroup = new TestFormRootGroup(RatingContributionTestData.EmptyContribution(), true);

        var contribution = rootGroup.Contribution();
        var total = contribution.Total();

        Assert.False(total.IsSome);
    }

    [Fact]
    public void Inherits_validation_behavior_from_group()
    {
        var rootGroup = new TestFormRootGroup(RatingContributionTestData.SingleContribution(), false);

        Assert.Throws<InvalidOperationException>(() => rootGroup.Validate());
    }

    [Fact]
    public void Returns_consistent_contribution_across_calls()
    {
        var expected = RatingContributionTestData.SingleContribution();
        var rootGroup = new TestFormRootGroup(expected, true);

        var first = rootGroup.Contribution();
        var second = rootGroup.Contribution();

        Assert.Equal(first, second);
    }
}

/// <summary>
/// Test double for form root group interface.
/// </summary>
file sealed record TestFormRootGroup(IRatingContribution TestContribution, bool IsValid) : IFormRootGroup
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
