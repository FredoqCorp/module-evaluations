using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IWeightedGroup behavioral contract.
/// </summary>
public sealed class WeightedGroupTests
{
    [Fact]
    public void Returns_associated_weight()
    {
        var group = new TestWeightedGroup(
            SharedTypesTestData.RandomWeight(),
            true);

        var weight = group.Weight();

        Assert.NotNull(weight);
    }

    [Fact]
    public void Validates_as_weighted_group()
    {
        var group = new TestWeightedGroup(
            SharedTypesTestData.RandomWeight(),
            true);

        var exception = Record.Exception(() => group.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Returns_consistent_weight_across_calls()
    {
        var expectedWeight = SharedTypesTestData.RandomWeight();
        var group = new TestWeightedGroup(
            expectedWeight,
            true);

        var first = group.Weight();
        var second = group.Weight();

        Assert.Equal(first, second);
    }

    [Fact]
    public void Inherits_validation_behavior_from_group()
    {
        var group = new TestWeightedGroup(
            SharedTypesTestData.RandomWeight(),
            false);

        Assert.Throws<InvalidOperationException>(() => group.Validate());
    }
}

/// <summary>
/// Test double for weighted group interface.
/// </summary>
file sealed record TestWeightedGroup(
    IWeight GroupWeight,
    bool IsValid) : IWeightedGroup
{
    public IWeight Weight() => GroupWeight;

    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
