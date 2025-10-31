using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Forms;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Forms;

/// <summary>
/// Tests for IFormRootGroup behavioral contract.
/// </summary>
public sealed class FormRootGroupTests
{
    [Fact]
    public void Validates_as_group()
    {
        var rootGroup = new TestFormRootGroup(true);

        var exception = Record.Exception(() => rootGroup.Validate());

        Assert.Null(exception);
    }

    [Fact]
    public void Inherits_validation_behavior_from_group()
    {
        var rootGroup = new TestFormRootGroup(false);

        Assert.Throws<InvalidOperationException>(() => rootGroup.Validate());
    }
}

/// <summary>
/// Test double for form root group interface.
/// </summary>
file sealed record TestFormRootGroup(bool IsValid) : IFormRootGroup
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
