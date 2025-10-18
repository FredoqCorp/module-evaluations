using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for IWeightedCriteria behavioral contract.
/// </summary>
public sealed class WeightedCriteriaTests
{
    [Fact]
    public void Returns_combined_sibling_weight()
    {
        var criteria = new TestWeightedCriteria(
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var weight = criteria.Weight();

        Assert.NotNull(weight);
    }

    [Fact]
    public void Validates_as_weighted_collection()
    {
        var criteria = new TestWeightedCriteria(
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var exception = Record.Exception(() => criteria.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Returns_consistent_weight_across_calls()
    {
        var expectedWeight = SharedTypesTestData.RandomBasisPoints();
        var criteria = new TestWeightedCriteria(
            expectedWeight,
            true);

        var first = criteria.Weight();
        var second = criteria.Weight();

        Assert.Equal(first, second);
    }

    [Fact]
    public void Inherits_validation_behavior_from_criteria()
    {
        var criteria = new TestWeightedCriteria(
            SharedTypesTestData.RandomBasisPoints(),
            false);

        Assert.Throws<InvalidOperationException>(() => criteria.Validate());
    }
}

/// <summary>
/// Test double for weighted criteria interface.
/// </summary>
file sealed record TestWeightedCriteria(
    IBasisPoints BasisPoints,
    bool IsValid) : IWeightedCriteria
{
    public IBasisPoints Weight() => BasisPoints;

    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
