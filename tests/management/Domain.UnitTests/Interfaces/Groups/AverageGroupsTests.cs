using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

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
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
