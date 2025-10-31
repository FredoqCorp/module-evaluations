using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for IAverageCriterion behavioral contract.
/// </summary>
public sealed class AverageCriterionTests
{
    [Fact]
    public void Validates_as_single_criterion()
    {
        var criterion = new TestAverageCriterion(true);

        var exception = Record.Exception(() => criterion.Validate());

        Assert.Null(exception);
    }
}

/// <summary>
/// Test double for average criterion interface.
/// </summary>
file sealed record TestAverageCriterion(bool IsValid) : IAverageCriterion
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
