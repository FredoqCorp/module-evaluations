using CascVel.Modules.Evaluations.Management.Domain.Interfaces.Criteria;

namespace CascVel.Modules.Evaluations.Management.Domain.UnitTests.Interfaces.Criteria;

/// <summary>
/// Tests for ICriterion behavioral contract.
/// </summary>
public sealed class CriterionTests
{
    [Fact]
    public void Validates_internal_consistency()
    {
        var criterion = new TestCriterion(true);

        var exception = Record.Exception(() => criterion.Validate());

        Assert.Null(exception);
    }
}

/// <summary>
/// Test double for criterion interface.
/// </summary>
file sealed record TestCriterion(bool IsValid) : ICriterion
{
    public void Validate()
    {
        if (!IsValid)
        {
            throw new InvalidOperationException("Validation failed");
        }
    }
}
