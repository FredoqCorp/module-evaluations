using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IAverageGroup behavioral contract.
/// </summary>
public sealed class AverageGroupTests
{
    [Fact]
    public void Validates_as_group()
    {
        var group = new TestAverageGroup(true);

        var exception = Record.Exception(() => group.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Inherits_validation_behavior_from_group()
    {
        var group = new TestAverageGroup(false);

        Assert.Throws<InvalidOperationException>(() => group.Validate());
    }
}

/// <summary>
/// Test double for average group interface.
/// </summary>
file sealed record TestAverageGroup(bool IsValid) : IAverageGroup
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
