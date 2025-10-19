using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;
using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Ratings;
using CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.TestFixtures;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IGroups behavioral contract.
/// </summary>
public sealed class GroupsTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var groups = new TestGroups(true);

        var exception = Record.Exception(() => groups.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var groups = new TestGroups(false);

        Assert.Throws<InvalidOperationException>(() => groups.Validate());
    }
}

/// <summary>
/// Test double for groups interface.
/// </summary>
file sealed record TestGroups(bool IsValid) : IGroups
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
