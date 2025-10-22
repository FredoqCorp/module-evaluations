using System;
using System.Threading;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Shared;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IWeightedGroups behavioral contract.
/// </summary>
public sealed class WeightedGroupsTests
{
    [Fact]
    public void Returns_combined_sibling_weight()
    {
        var groups = new TestWeightedGroups(
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var weight = groups.Weight();

        Assert.NotNull(weight);
    }

    [Fact]
    public void Validates_as_weighted_collection()
    {
        var groups = new TestWeightedGroups(
            SharedTypesTestData.RandomBasisPoints(),
            true);

        var exception = Record.Exception(() => groups.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Returns_consistent_weight_across_calls()
    {
        var expectedWeight = SharedTypesTestData.RandomBasisPoints();
        var groups = new TestWeightedGroups(
            expectedWeight,
            true);

        var first = groups.Weight();
        var second = groups.Weight();

        Assert.Equal(first, second);
    }

    [Fact]
    public void Inherits_validation_behavior_from_groups()
    {
        var groups = new TestWeightedGroups(
            SharedTypesTestData.RandomBasisPoints(),
            false);

        Assert.Throws<InvalidOperationException>(() => groups.Validate());
    }
}

/// <summary>
/// Test double for weighted groups interface.
/// </summary>
file sealed record TestWeightedGroups(
    IBasisPoints BasisPoints,
    bool IsValid) : IWeightedGroups
{
    /// <summary>
    /// Adds a new weighted group under a form.
    /// </summary>
    public Task<IWeightedGroup> Add(GroupProfile profile, FormId formId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(weight);

        IWeightedGroup group = new TestWeightedGroup(weight);
        return Task.FromResult(group);
    }

    /// <summary>
    /// Adds a new weighted group under another group.
    /// </summary>
    public Task<IWeightedGroup> Add(GroupProfile profile, GroupId parentId, IWeight weight, OrderIndex orderIndex, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(weight);

        IWeightedGroup group = new TestWeightedGroup(weight);
        return Task.FromResult(group);
    }

    /// <summary>
    /// Returns the combined sibling weight.
    /// </summary>
    public IBasisPoints Weight() => BasisPoints;

    /// <summary>
    /// Validates the test group collection.
    /// </summary>
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }

    /// <summary>
    /// Test double for weighted group interface.
    /// </summary>
    private sealed record TestWeightedGroup(IWeight WeightValue) : IWeightedGroup
    {
        /// <summary>
        /// Returns the test weight.
        /// </summary>
        public IWeight Weight() => WeightValue;

        /// <summary>
        /// Validates the test weighted group.
        /// </summary>
        public void Validate()
        {
        }
    }
}
