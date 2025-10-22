using System.Threading;
using System.Threading.Tasks;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Forms;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Groups;
using CascVel.Modules.Evaluations.Management.Domain.ValueObjects.Shared;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IAverageGroups behavioral contract.
/// </summary>
public sealed class AverageGroupsTests
{
    [Fact]
    public void Validates_as_groups_collection()
    {
        var groups = new TestAverageGroups(true);

        var exception = Record.Exception(() => groups.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Inherits_validation_behavior_from_groups()
    {
        var groups = new TestAverageGroups(false);

        Assert.Throws<InvalidOperationException>(() => groups.Validate());
    }
}

/// <summary>
/// Test double for average groups interface.
/// </summary>
file sealed record TestAverageGroups(bool IsValid) : IAverageGroups
{
    /// <summary>
    /// Adds a new average group under a form.
    /// </summary>
    public Task<IAverageGroup> Add(GroupProfile profile, FormId formId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        IAverageGroup group = new TestAverageGroup();
        return Task.FromResult(group);
    }

    /// <summary>
    /// Adds a new average group under another group.
    /// </summary>
    public Task<IAverageGroup> Add(GroupProfile profile, GroupId parentId, OrderIndex orderIndex, CancellationToken ct = default)
    {
        IAverageGroup group = new TestAverageGroup();
        return Task.FromResult(group);
    }

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
    /// Test double for average group interface.
    /// </summary>
    private sealed record TestAverageGroup : IAverageGroup
    {
        /// <summary>
        /// Validates the test group.
        /// </summary>
        public void Validate()
        {
        }
    }
}
