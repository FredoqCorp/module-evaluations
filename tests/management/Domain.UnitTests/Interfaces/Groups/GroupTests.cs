using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Groups;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Groups;

/// <summary>
/// Tests for IGroup behavioral contract.
/// </summary>
public sealed class GroupTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var group = new TestGroup(true);

        var exception = Record.Exception(() => group.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Throws_when_validation_fails()
    {
        var group = new TestGroup(false);

        Assert.Throws<InvalidOperationException>(() => group.Validate());
    }
}

/// <summary>
/// Test double for group interface.
/// </summary>
file sealed record TestGroup(bool IsValid) : IGroup
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
